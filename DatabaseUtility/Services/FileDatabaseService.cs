using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DatabaseUtility.Models;
using ReactiveUI;
using Splat;

namespace DatabaseUtility.Services;

public class FileDatabaseService : ReactiveObject, IDatabaseService
{
    private static string SaveFile => Path.Combine(App.DataPath, "databases.json");
    
    public IEnumerable<DatabaseConnection> GetDatabases()
    {
        this.Log().Info($"Loading databases from '{SaveFile}'.");
        try
        {
            string json = File.ReadAllText(SaveFile);
            return JsonSerializer.Deserialize<List<DatabaseConnection>>(json) ?? Enumerable.Empty<DatabaseConnection>();

        }
        catch (Exception e)
        {
            this.Log().Error(e, "Unable to load databases file");
            return Enumerable.Empty<DatabaseConnection>();
        }
    }

    public void SaveDatabases(IEnumerable<DatabaseConnection> databases)
    {
        this.Log().Info($"Writing databases to '{SaveFile}'.");
        try
        {
            string json = JsonSerializer.Serialize(databases.ToList());
            File.WriteAllText(SaveFile, json);
        }
        catch (Exception e)
        {
            this.Log().Error(e, "Unable to write databases file");
        }
    }
}