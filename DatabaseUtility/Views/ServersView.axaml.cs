using Avalonia.ReactiveUI;
using DatabaseUtility.ViewModels;

namespace DatabaseUtility.Views;

public partial class ServersView : ReactiveUserControl<ServersViewModel>
{
    public ServersView()
    {
        InitializeComponent();
    }
}