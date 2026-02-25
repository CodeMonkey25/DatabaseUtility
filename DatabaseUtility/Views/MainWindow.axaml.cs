using DatabaseUtility.ViewModels;
using ReactiveUI.Avalonia;

namespace DatabaseUtility.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}