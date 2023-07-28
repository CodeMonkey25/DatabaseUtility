using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DatabaseUtility.ViewModels;
using DialogHostAvalonia;

namespace DatabaseUtility.Views;

public partial class EditDatabaseView : UserControl
{
    private EditDatabaseViewModel ViewModel => DataContext as EditDatabaseViewModel ?? throw new InvalidOperationException();
    
    public EditDatabaseView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ViewModel.SaveChanges();
        DialogHost.GetDialogSession(null)?.Close(true);
    }
}