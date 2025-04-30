using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Dapper;
using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.Utility;
using DynamicData;
using MySqlConnector;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;

namespace DatabaseUtility.ViewModels;

public partial class DataInfoViewModel : ViewModelBase
{
    public override ViewTypes ViewType => ViewTypes.DataInfo;
    
    private readonly ISettingsService? _settingsService;
    private readonly IObservable<bool>? _canUpdateDataInfoExecute;
    
    [Reactive] private string _dataInfoFilePath = @"C:\Users\jsarley\source\repos\rm\UI\Desktop\WPFClient\bin\Debug\datainfo.dat";
    [Reactive] private ObservableCollection<string> _databaseServers = [];
    [Reactive] private ObservableCollection<string> _databaseNames = [];
    [Reactive] private string? _selectedDatabaseServer;
    [Reactive] private string? _selectedDatabaseName;
    
    public DataInfoViewModel() { /* constructor for axaml designer */ }
    
    [DependencyInjectionConstructor]
    public DataInfoViewModel(ISettingsService? settingsService)
    {
        _settingsService = settingsService ?? throw new Exception("settings service is required");
        Settings settings = _settingsService.Get();
        _databaseServers.Clear();
        _databaseServers.Add(settings.DatabaseServers);

        this.WhenAnyValue(x => x.SelectedDatabaseServer)
            .Subscribe(LoadDatabases);

        _canUpdateDataInfoExecute = this.WhenAnyValue(x => x.SelectedDatabaseName).Select(x => x != null);
        
        LoadDataInfo();
    }

    private void LoadDataInfo()
    {
        try
        {
            string server = string.Empty;
            string db = string.Empty;
            
            // load from file
            foreach (string line in File.ReadLines(DataInfoFilePath))
            {
                if (line.StartsWith("database="))
                {
                    db = line.Split("=")[1].Trim();
                }
                else if (line.StartsWith("server="))
                {
                    server = line.Split("=")[1].Trim();
                }
            }
            
            // validate data loaded
            if (string.IsNullOrEmpty(server)) throw new Exception("Unable to locate server record.");
            if (string.IsNullOrEmpty(db)) throw new Exception("Unable to locate database record.");
            
            // select server record
            SelectedDatabaseServer = DatabaseServers.FirstOrDefault(s => s == server);
            
            // select db record
            SelectedDatabaseName = DatabaseNames.FirstOrDefault(n => n.Equals($"{db}_rm12", StringComparison.InvariantCultureIgnoreCase));
        }
        catch (Exception e)
        {
            this.Log().Error(e, "Unable to load datainfo file");
        }
    }
    
    private void LoadDatabases(string? serverName)
    {
        DatabaseNames.Clear();
        SelectedDatabaseName = null;
        if (serverName == null) { return; }
        
        Settings settings = _settingsService!.Get();
        string databaseNameFilter = settings.DatabaseNameFilter;
        bool showOnlyRM12Databases = settings.ShowOnlyRM12Databases;
        bool showOnlyDefaultLocations = settings.ShowOnlyDefaultLocations;
        
        try
        {
            IEnumerable<string> databases = Enumerable.Empty<string>();
            MySqlConnectionStringBuilder builder = new()
            {
                Server = serverName,
                Port = 3306,
                UserID = "root",
                Password = "1010as",
            };
            using (IDbConnection conn = new MySqlConnection(builder.ToString()))
            {
                conn.Open();
                const string sql = "SELECT SCHEMA_NAME FROM information_schema.schemata WHERE `SCHEMA_NAME` NOT IN ('information_schema', 'mysql', 'performance_schema', 'sys') ORDER BY `SCHEMA_NAME`";
                databases = conn.Query<string>(sql);
            }

            if (!string.IsNullOrEmpty(databaseNameFilter))
            {
                databases = databases.Where(name => name.Contains(databaseNameFilter, StringComparison.CurrentCultureIgnoreCase));
            }

            if (showOnlyRM12Databases)
            {
                databases = databases.Where(name => name.EndsWith("_rm12", StringComparison.CurrentCultureIgnoreCase));
                if (showOnlyDefaultLocations)
                {
                    databases = databases.Where(name => name.Count(c => c == '_') == 1);
                }
            }

            DatabaseNames.AddRange(databases);
        }
        catch (Exception e)
        {
            this.Log().Error(e, "Unable to load database names");
        }
    }

    
    [ReactiveCommand(CanExecute = nameof(_canUpdateDataInfoExecute))]
    private void UpdateDataInfo()
    {
        if (SelectedDatabaseServer == null) return;
        if (SelectedDatabaseName == null) return;

        try
        {
            string[] lines = File.ReadAllLines(DataInfoFilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("database="))
                {
                    lines[i] = $"database={SelectedDatabaseName[..^5]}";
                }
                else if (lines[i].StartsWith("server="))
                {
                    lines[i] = $"server={SelectedDatabaseServer}";
                }
            }

            File.WriteAllLines(DataInfoFilePath, lines);
        }
        catch (Exception e)
        {
            this.Log().Error(e, "Unable to update datainfo file");
        }
    }
}