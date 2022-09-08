using System;
using Avalonia;
using Avalonia.ReactiveUI;
using DesktopApp.Models;
using DesktopApp.Services;
using Serilog;
using Splat;
using Splat.Serilog;

namespace DesktopApp
{
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
        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        private static void SetUpLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(Settings.LogFile, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Locator.CurrentMutable.UseSerilogFullLogger();
        }

        private static void RegisterDependencies()
        {
            Locator.CurrentMutable.Register<ISettingsService>(() => new FileSettingsService());
        }
    }
}