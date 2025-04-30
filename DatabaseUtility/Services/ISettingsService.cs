using DatabaseUtility.Models;

namespace DatabaseUtility.Services;

public interface ISettingsService
{
    public Settings Get();
    public void Save(Settings settings);
}