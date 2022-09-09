using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DesktopApp.Extensions;
using DesktopApp.Models;
using DesktopApp.Services;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Splat;

namespace DesktopApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly Settings _settings;
        private readonly ReadOnlyObservableCollection<Connection> _connections;

        private Settings Settings
        {
            get => _settings;
            init => this.RaiseAndSetIfChanged(ref _settings, value);
        }

        internal ReadOnlyObservableCollection<Connection> Connections
        {
            get => _connections;
            private init => this.RaiseAndSetIfChanged(ref _connections, value);
        }

        public SettingsViewModel()
        {
            _settings = Settings = Locator.Current.GetRequiredService<Settings>();
            _connections = Connections = new ReadOnlyObservableCollection<Connection>(Settings.Connections);
            
            this.WhenActivated(disposables =>
                {
                    Connections.ToObservableChangeSet()
                        .AutoRefresh()
                        .Throttle(TimeSpan.FromSeconds(1))
                        .Subscribe(_ => this.Save(), this.Log().Error)
                        .DisposeWith(disposables);
                }
            );
        }
        
        internal void AddConnection()
        {
            ISettingsService settingsService = Locator.Current.GetRequiredService<ISettingsService>();
            settingsService.AddConnection();
        }

        internal void RemoveConnection(Connection connection)
        {
            ISettingsService settingsService = Locator.Current.GetRequiredService<ISettingsService>();
            settingsService.RemoveConnection(connection.Id);
        }

        private void Save()
        {
            ISettingsService settingsService = Locator.Current.GetRequiredService<ISettingsService>();
            settingsService.Save();
        }
    }
}