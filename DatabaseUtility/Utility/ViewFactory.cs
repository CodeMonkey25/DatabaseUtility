using System;
using DatabaseUtility.ViewModels;
using Splat;

namespace DatabaseUtility.Utility;

public class ViewFactory : IViewFactory
{
    public ViewModelBase? CreateView(ViewTypes viewType)
    {
        return viewType switch
        {
            ViewTypes.Main => Locator.Current.GetService<MainWindowViewModel>(),
            ViewTypes.Settings => Locator.Current.GetService<SettingsViewModel>(),
            ViewTypes.DataInfo => Locator.Current.GetService<DataInfoViewModel>(),
            ViewTypes.Servers => Locator.Current.GetService<ServersViewModel>(),
            _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null)
        };
    }
}