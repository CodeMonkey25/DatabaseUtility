using Avalonia.ReactiveUI;
using DatabaseUtility.ViewModels;

namespace DatabaseUtility.Views;

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();
    }
}