using DatabaseUtility.ViewModels;
using ReactiveUI.Avalonia;

namespace DatabaseUtility.Views;

public partial class DataInfoView : ReactiveUserControl<DataInfoViewModel>
{
    public DataInfoView()
    {
        InitializeComponent();
    }
}