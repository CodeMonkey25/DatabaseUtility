using System.Runtime.Serialization;
using ReactiveUI;

namespace DesktopApp.Models
{
    [DataContract]
    public class Connection : ReactiveObject
    {
        private string _name = string.Empty;
        private string _serverIpAddress = string.Empty;
        private uint _serverPort = 0;
        private string _databaseUser = string.Empty;
        private string _databasePassword = string.Empty;

        [DataMember] public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
        [DataMember] public string ServerIpAddress { get => _serverIpAddress; set => this.RaiseAndSetIfChanged(ref _serverIpAddress, value); }
        [DataMember] public uint ServerPort { get => _serverPort; set => this.RaiseAndSetIfChanged(ref _serverPort, value); }
        [DataMember] public string DatabaseUser { get => _databaseUser; set => this.RaiseAndSetIfChanged(ref _databaseUser, value); }
        [DataMember] public string DatabasePassword { get => _databasePassword; set => this.RaiseAndSetIfChanged(ref _databasePassword, value); }
    }
}