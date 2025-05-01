using System.Collections.Generic;

namespace DatabaseUtility.Models;

public class Settings
{
    public bool ShowOnlyRM12Databases { get; set; }
    public bool ShowOnlyDefaultLocations { get; set; }
    public string DatabaseNameFilter { get; set; } = string.Empty;
    public string DatabaseUser { get; set; } = string.Empty;
    public string DatabasePassword { get; set; } = string.Empty;
    public List<string> DatabaseServers { get; set; } = [];
}