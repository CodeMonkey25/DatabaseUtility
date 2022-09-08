using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopApp.ViewModels;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace DesktopApp.Views
{
    public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        private ListBox ConnectionListBox => this.FindControl<ListBox>("ConnectionListBox");

        public SettingsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            

            this.WhenActivated(
                disposables =>
                {
                    this.WhenAnyValue(x => x.ViewModel)
                        .WhereNotNull()
                        .Subscribe(vm => PopulateFromViewModel(vm, disposables))
                        .DisposeWith(disposables);
                }
            );
        }
        
        private void PopulateFromViewModel(SettingsViewModel vm, CompositeDisposable disposables)
        {
            bool initialized = false;
            vm.Connections
                .ToObservableChangeSet()
                .Where(_ => initialized)
                .ObserveOn(RxApp.MainThreadScheduler)
                .OnItemAdded(a => ConnectionListBox.SelectedItem = a)
                .Subscribe()
                .DisposeWith(disposables);

            RxApp.MainThreadScheduler
                .Schedule(
                    () =>
                    {
                        if (ConnectionListBox.ItemCount > 0) ConnectionListBox.SelectedIndex = 0;
                        initialized = true;
                    }
                )
                .DisposeWith(disposables);
        }
    }
}