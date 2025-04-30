using Avalonia.ReactiveUI;
using DatabaseUtility.ViewModels;

namespace DatabaseUtility.Views;

public partial class DataInfoView : ReactiveUserControl<DataInfoViewModel>
{
    public DataInfoView()
    {
        InitializeComponent();
    }
}