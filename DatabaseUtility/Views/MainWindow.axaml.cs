using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DatabaseUtility.ViewModels;
using Material.Styles.Controls;
using Material.Styles.Models;

namespace DatabaseUtility.Views;

public partial class MainWindow : Window
{
    private readonly List<SnackbarModel> _helloSnackBars = new();

    private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel ?? throw new InvalidOperationException();
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TemplatedControl_OnTemplateApplied(object? sender, TemplateAppliedEventArgs e)
    {
        SnackbarHost.Post("Welcome to DatabaseUtility!", null, DispatcherPriority.Normal);
    }

    private void HelloButtonMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        var helloSnackBar = new SnackbarModel("Hello, user!", TimeSpan.Zero);
        SnackbarHost.Post(helloSnackBar, null, DispatcherPriority.Normal);
        _helloSnackBars.Add(helloSnackBar);
    }

    private void GoodbyeButtonMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        foreach (var snackbarModel in _helloSnackBars) {
            SnackbarHost.Remove(snackbarModel, null, DispatcherPriority.Normal);
        }

        SnackbarHost.Post("See ya next time, user!", null, DispatcherPriority.Normal);
    }

    private void DrawerList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox listBox) return;
        if (listBox.SelectedItem is not TextBlock textBlock) return;

        switch (textBlock.Text)
        {
            case "Database Servers":
                if (ViewModel.MySubViewModel is not ListDatabaseViewModel)
                    ViewModel.MySubViewModel = new ListDatabaseViewModel();
                break;
            case "DataInfo File":
                if (ViewModel.MySubViewModel is not DataInfoFileViewModel)
                    ViewModel.MySubViewModel = new DataInfoFileViewModel();
                break;
            default:
                SnackbarHost.Post($"unknown menu choice '{textBlock.Text}'!", null, DispatcherPriority.Normal);
                listBox.SelectedIndex = 0;
                break;
        }
        NavDrawerSwitch.IsChecked = false;
    }
}