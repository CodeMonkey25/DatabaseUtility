using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DatabaseUtility.ViewModels;
using DatabaseUtility.Views;

namespace DatabaseUtility;

public partial class App : Application
{
    // Environment.SpecialFolder.CommonApplicationData => C:\ProgramData
    // Environment.SpecialFolder.ApplicationData => C:\Users\jsarley\AppData\Roaming
    // Environment.SpecialFolder.LocalApplicationData => C:\Users\jsarley\AppData\Local

    internal static string LocalApplicationDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    internal static string JmsDataPath => Path.Combine(LocalApplicationDataPath, "JMS");
    internal static string DataPath => Path.Combine(JmsDataPath, "DatabaseUtility");
    internal static string LogFile => Path.Combine(DataPath, "Logs", "daily_.log");
    
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
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}