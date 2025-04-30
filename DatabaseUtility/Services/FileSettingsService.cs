using System;
using System.IO;
using System.Text.Json;
using DatabaseUtility.Models;
using DatabaseUtility.Utility;

namespace DatabaseUtility.Services;

public class FileSettingsService : ISettingsService
{
    private static string SettingsFile => Path.Combine(Constants.DataPath, "settings.json");

    private readonly ILoggerService _logger;

    public FileSettingsService(ILoggerService logger)
    {
        _logger = logger;
    }

    public Settings Get()
    {
        _logger.LogInfo($"Loading settings from '{SettingsFile}'.");
        try
        {
            string json = File.ReadAllText(SettingsFile);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to load settings file");
            return new Settings();
        }
    }

    public void Save(Settings settings)
    {
        _logger.LogInfo($"Saving settings to '{SettingsFile}'.");
        try
        {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(SettingsFile, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to save settings file.");
            throw;
        }
    }
}