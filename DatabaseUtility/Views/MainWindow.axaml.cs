using Avalonia.ReactiveUI;
using DatabaseUtility.ViewModels;

namespace DatabaseUtility.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}