using System;
using System.IO;
using DesktopApp.Models;
using Newtonsoft.Json;
using ReactiveUI;
using Splat;

namespace DesktopApp.Services
{
    public class FileSettingsService : ReactiveObject, ISettingsService
    {
        internal static string SettingsFile => Path.Combine(Settings.DataPath, "settings.json");

        public Settings Load()
        {
            if (!File.Exists(SettingsFile))
            {
                this.Log().Warn("Settings file missing, restoring defaults");
                return new Settings();
            }
            
            if (JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsFile)) is Settings settings)
            {
                return settings;
            }

            this.Log().Warn("Settings file corrupt, restoring defaults");
            return new Settings();
        }

        public void Save(Settings settings)
        {
            try
            {
                File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(settings, Formatting.Indented));
            }
            catch (Exception ex)
            {
                this.Log().Error(ex, "Unable to save settings file.");
                throw;
            }
        }
    }
}