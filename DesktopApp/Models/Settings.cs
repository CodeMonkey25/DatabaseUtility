using System;
using System.IO;
using System.Runtime.Serialization;
using DynamicData.Binding;
using ReactiveUI;

namespace DesktopApp.Models
{
    [DataContract]
    public class Settings : ReactiveObject
    {
        // Environment.SpecialFolder.CommonApplicationData => C:\ProgramData
        // Environment.SpecialFolder.ApplicationData => C:\Users\jsarley\AppData\Roaming
        // Environment.SpecialFolder.LocalApplicationData => C:\Users\jsarley\AppData\Local

        internal static string LocalApplicationDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        internal static string JmsDataPath => Path.Combine(LocalApplicationDataPath, "JMS");
        internal static string DataPath => Path.Combine(JmsDataPath, "DatabaseUtility");
        internal static string LogFile => Path.Combine(DataPath, "Logs", "daily_.log");

        private ObservableCollectionExtended<Connection> _connections = new();
        [DataMember] public ObservableCollectionExtended<Connection> Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }
    }
}