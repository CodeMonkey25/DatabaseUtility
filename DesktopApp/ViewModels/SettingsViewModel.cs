using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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
            ISettingsService settingsService = Locator.Current.GetService<ISettingsService>() ?? throw new Exception("unable to load ISettingsService");
            Settings = settingsService.Load();
            Connections = new ReadOnlyObservableCollection<Connection>(Settings.Connections);
            
            this.WhenActivated((CompositeDisposable disposables) =>
                {
                    Connections.ToObservableChangeSet()
                        .AutoRefresh()
                        .Throttle(TimeSpan.FromSeconds(1))
                        .Subscribe(_ => this.Save(), e => this.Log().Error(e))
                        .DisposeWith(disposables);
                }
            );
        }
        
        internal void AddConnection()
        {
            this.Log().Info("adding new connection");

            int i = 0;
            string name;
            do
            {
                name = $"New Connection {++i}";
            } while (Settings.Connections.Any(cs => String.Equals(cs.Name, name, StringComparison.CurrentCultureIgnoreCase)));

            Settings.Connections.Add(new Connection() { Name = name, ServerPort = 3306 });
        }

        internal void RemoveConnection(int index)
        {
            this.Log().Info($"removing existing connection {index}");
            Settings.Connections.RemoveAt(index);
        }
        
        internal void Save()
        {
            this.Log().Info("saving settings");
            ISettingsService settingsService = Locator.Current.GetService<ISettingsService>() ?? throw new Exception("unable to load ISettingsService");
            settingsService.Save(Settings);
        }
    }
}