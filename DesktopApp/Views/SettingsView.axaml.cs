using System;
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
        public SettingsView()
        {
            InitializeComponent();
            ConnectionListBox = this.FindControl<ListBox>("ConnectionListBox");
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
            vm.Connections
                .ToObservableChangeSet()
                .ObserveOn(RxApp.MainThreadScheduler)
                .OnItemAdded(a => ConnectionListBox.SelectedItem = a)
                .Subscribe()
                .DisposeWith(disposables);
        }

        private void SettingsView_OnInitialized(object? sender, EventArgs e)
        {
            if (ConnectionListBox.ItemCount > 0) ConnectionListBox.SelectedIndex = 0;
        }
    }
}