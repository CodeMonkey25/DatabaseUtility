using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DatabaseUtility.ViewModels;
using DialogHostAvalonia;
using ReactiveUI;

namespace DatabaseUtility.Views;

public partial class EditDatabaseView : UserControl
{
    public static readonly DirectProperty<EditDatabaseView, ICommand> SaveCommandProperty = AvaloniaProperty.RegisterDirect<EditDatabaseView, ICommand>(nameof(SaveCommand), o => o.SaveCommand, (o, v) => o.SaveCommand = v);
    private ICommand _saveCommand = ReactiveCommand.Create(() => { });
    public ICommand SaveCommand
    {
        get => _saveCommand;
        set => SetAndRaise(SaveCommandProperty, ref _saveCommand, value);
    }
    public EditDatabaseView()
    {
        InitializeComponent();

        this.WhenAnyValue(x => x.DataContext)
            .WhereNotNull()
            .OfType<EditDatabaseViewModel>()
            .Subscribe(vm =>
            {
                SaveCommand = ReactiveCommand.Create(SaveChanges, vm.CanSaveChanges);
            });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SaveChanges()
    {
        if (DataContext is not EditDatabaseViewModel viewModel) return;

        viewModel.SaveChanges();
        DialogHost.GetDialogSession(null)?.Close(true);
    }
}