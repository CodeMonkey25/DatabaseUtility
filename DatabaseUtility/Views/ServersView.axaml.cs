using DatabaseUtility.ViewModels;
using ReactiveUI.Avalonia;

namespace DatabaseUtility.Views;

public partial class ServersView : ReactiveUserControl<ServersViewModel>
{
    public ServersView()
    {
        InitializeComponent();
    }
}