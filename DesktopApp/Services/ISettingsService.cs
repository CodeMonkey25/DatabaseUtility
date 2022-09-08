using DesktopApp.Models;

namespace DesktopApp.Services
{
    public interface ISettingsService
    {
        public Settings Load();
        public void Save(Settings settings);
    }
}