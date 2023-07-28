using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DatabaseUtility.Models;
using DatabaseUtility.ViewModels;
using DialogHostAvalonia;
using Material.Styles.Controls;

namespace DatabaseUtility.Views;

public partial class ListDatabaseView : UserControl
{
    private ListDatabaseViewModel ViewModel => DataContext as ListDatabaseViewModel ?? throw new InvalidOperationException();
    
    public ListDatabaseView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        DatabaseConnection db = new DatabaseConnection();
        object? resultObj = await DialogHost.Show(new EditDatabaseViewModel(db));
        if (resultObj is not bool result) return;
        if (!result) return;
        
        ViewModel.AddDatabase(db);
    }

    private async void InputElement_OnTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not Card card) return;
        if (card.DataContext is not DatabaseConnection database) return;
        
        object? resultObj = await DialogHost.Show(new EditDatabaseViewModel(database));
        if (resultObj is not bool result) return;
        if (!result) return;

        ViewModel.SaveChanges();
    }
}