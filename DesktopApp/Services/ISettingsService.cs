using System;

namespace DesktopApp.Services
{
    public interface ISettingsService
    {
        public void Load();
        public void Save();

        public void AddConnection();
        public void RemoveConnection(Guid id);
    }
}