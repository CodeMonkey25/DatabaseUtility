using Avalonia;
using Avalonia.ReactiveUI;
using System;
using DatabaseUtility.Services;
using DatabaseUtility.Utility;
using DatabaseUtility.ViewModels;
using Serilog;
using Splat;
using Splat.Serilog;

namespace DatabaseUtility;

internal static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        SetUpLogging();
        RegisterDependencies();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    
    private static void SetUpLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(Constants.LogFile, rollingInterval: RollingInterval.Month)
            .CreateLogger();

        Locator.CurrentMutable.UseSerilogFullLogger();
    }

    private static void RegisterDependencies()
    {
        SplatRegistrations.Register<IViewFactory, ViewFactory>();
        SplatRegistrations.Register<ILoggerService, LoggerService>();
        SplatRegistrations.Register<ISettingsService, FileSettingsService>();
        SplatRegistrations.Register<IDatabaseService, DatabaseService>();
        SplatRegistrations.Register<MainWindowViewModel>();
        SplatRegistrations.Register<SettingsViewModel>();
        SplatRegistrations.Register<DataInfoViewModel>();
        SplatRegistrations.Register<ServersViewModel>();
        
        SplatRegistrations.SetupIOC();
    }
}