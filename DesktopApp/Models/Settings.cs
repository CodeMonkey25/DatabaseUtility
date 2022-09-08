using System;
using System.Collections.ObjectModel;
using System.IO;
using ReactiveUI;

namespace DesktopApp.Models
{
    public class Settings : ReactiveObject
    {
        // Environment.SpecialFolder.CommonApplicationData => C:\ProgramData
        // Environment.SpecialFolder.ApplicationData => C:\Users\jsarley\AppData\Roaming
        // Environment.SpecialFolder.LocalApplicationData => C:\Users\jsarley\AppData\Local

        internal static string LocalApplicationDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        internal static string JmsDataPath => Path.Combine(LocalApplicationDataPath, "JMS");
        internal static string DataPath => Path.Combine(JmsDataPath, "DatabaseUtility");
        internal static string LogFile => Path.Combine(DataPath, "Logs", "daily_.log");

        private ObservableCollection<Connection> _connections = new();
        public ObservableCollection<Connection> Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }
    }
}