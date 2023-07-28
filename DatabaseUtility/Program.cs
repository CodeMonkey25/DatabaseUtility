using Avalonia;
using Avalonia.ReactiveUI;
using System;
using DatabaseUtility.Services;
using Serilog;
using Splat;
using Splat.Serilog;

namespace DatabaseUtility;

class Program
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
            .WriteTo.File(App.LogFile, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Locator.CurrentMutable.UseSerilogFullLogger();
    }

    private static void RegisterDependencies()
    {
        Locator.CurrentMutable.Register<IDatabaseService>(() => new FileDatabaseService());
    }
}