using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesktopApp.Extensions;
using DesktopApp.Models;
using Newtonsoft.Json;
using ReactiveUI;
using Splat;

namespace DesktopApp.Services
{
    public class FileSettingsService : ReactiveObject, ISettingsService
    {
        private static string SettingsFile => Path.Combine(Settings.DataPath, "settings.json");

        public void Load()
        {
            this.Log().Info($"Loading settings from '{SettingsFile}'.");
            Settings settings = Locator.Current.GetRequiredService<Settings>();
            try
            {
                JsonConvert.PopulateObject(File.ReadAllText(SettingsFile), settings);
            }
            catch (Exception e)
            {
                this.Log().Error(e, "Unable to load settings file, restoring defaults");
                JsonConvert.PopulateObject(
                    JsonConvert.SerializeObject(new Settings()),
                    settings
                );
            }
        }

        public void Save()
        {
            this.Log().Info("Saving settings to '{SettingsFile}'.");
            Settings settings = Locator.Current.GetRequiredService<Settings>();
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

        public void AddConnection()
        {
            this.Log().Info("Adding new connection.");

            const string namePrefix = "New Connection ";
            Settings settings = Locator.Current.GetRequiredService<Settings>();

            // find all used numeric suffixes
            HashSet<int> used = settings.Connections
                .Select(c => c.Name.ToLower().Trim())
                .Distinct()
                .Where(s => s.StartsWith(namePrefix.ToLower()))
                .Select(s => s.Substring(namePrefix.Length))
                .Select(s => s.Trim())
                .Where(s => s != string.Empty)
                .Where(s => s.All(char.IsDigit))
                .Where(s => s.Length < 10)
                .Select(int.Parse)
                .ToHashSet();

            // find first unused numeric suffix
            int nameSuffix = Enumerable.Range(1, used.Count + 1).First(i => !used.Contains(i));

            // create new connection
            settings.Connections.Add(
                new Connection()
                {
                    Id = Guid.NewGuid(),
                    Name = namePrefix + nameSuffix,
                    ServerPort = 3306
                }
            );
        }

        public void RemoveConnection(Guid id)
        {
            this.Log().Info($"Removing existing connection id {id}");
            Settings settings = Locator.Current.GetRequiredService<Settings>();
            settings.Connections.RemoveAll(c => c.Id == id);
        }
    }
}