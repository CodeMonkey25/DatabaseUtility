using DatabaseUtility.ViewModels;

namespace DatabaseUtility.Utility;

public interface IViewFactory
{
    ViewModelBase? CreateView(ViewTypes viewType);
}