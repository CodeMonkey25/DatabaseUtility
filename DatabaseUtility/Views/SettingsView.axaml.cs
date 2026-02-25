using DatabaseUtility.ViewModels;
using ReactiveUI.Avalonia;

namespace DatabaseUtility.Views;

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();
    }
}