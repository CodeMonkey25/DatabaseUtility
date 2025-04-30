using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DatabaseUtility.Utility;
using DatabaseUtility.Views;
using Splat;

[assembly:InternalsVisibleTo("DatabaseUtility.Tests")]

namespace DatabaseUtility;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Locator.Current.GetService<IViewFactory>()!.CreateView(ViewTypes.Main),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}