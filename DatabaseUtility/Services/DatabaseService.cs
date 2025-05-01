using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DatabaseUtility.Models;
using MySqlConnector;

namespace DatabaseUtility.Services;

public class DatabaseService(ISettingsService? settingsService) : IDatabaseService
{
    public List<string> GetDatabaseNames(string server)
    {
        Settings settings = settingsService!.Get();
        string databaseNameFilter = settings.DatabaseNameFilter;
        bool showOnlyRM12Databases = settings.ShowOnlyRM12Databases;
        bool showOnlyDefaultLocations = settings.ShowOnlyDefaultLocations;

        IEnumerable<string> databases;
        MySqlConnectionStringBuilder builder = new()
        {
            Server = server,
            Port = 3306,
            UserID = settings.DatabaseUser,
            Password = settings.DatabasePassword,
        };
        using (IDbConnection conn = new MySqlConnection(builder.ToString()))
        {
            conn.Open();
            const string sql = "SELECT SCHEMA_NAME FROM information_schema.schemata WHERE `SCHEMA_NAME` NOT IN ('information_schema', 'mysql', 'performance_schema', 'sys') ORDER BY `SCHEMA_NAME`";
            databases = conn.Query<string>(sql);
        }

        if (!string.IsNullOrEmpty(databaseNameFilter))
        {
            databases = databases.Where(name =>
                name.Contains(databaseNameFilter, StringComparison.CurrentCultureIgnoreCase));
        }

        if (showOnlyRM12Databases)
        {
            databases = databases.Where(name => name.EndsWith("_rm12", StringComparison.CurrentCultureIgnoreCase));
            if (showOnlyDefaultLocations)
            {
                databases = databases.Where(name => name.Count(c => c == '_') == 1);
            }
        }
        
        return databases.ToList();
    }
}