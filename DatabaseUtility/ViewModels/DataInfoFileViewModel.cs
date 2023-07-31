using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Dapper;
using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DynamicData;
using DatabaseUtility.Extensions;
using MySqlConnector;
using ReactiveUI;
using Splat;

namespace DatabaseUtility.ViewModels;

public class DataInfoFileViewModel : ViewModelBase
{
    private string _dataInfoFilePath = @"C:\Users\jsarley\source\repos\rm\UI\Desktop\WPFClient\bin\Debug\datainfo.dat";
    private ObservableCollection<DatabaseConnection> _databaseConnections = new();
    private ObservableCollection<string> _databaseNames = new();
    private DatabaseConnection? _selectedDatabaseConnection;
    private string? _selectedDatabaseName;
    private bool _showOnlyRM12Databases = true;
    private bool _showOnlyDefaultLocations = true;
    private string _databaseNameFilter = "jsarley";

    public string DataInfoFilePath
    {
        get => _dataInfoFilePath;
        set => this.RaiseAndSetIfChanged(ref _dataInfoFilePath, value);
    }
    
    public ObservableCollection<DatabaseConnection> DatabaseConnections
    {
        get => _databaseConnections;
        set => this.RaiseAndSetIfChanged(ref _databaseConnections, value);
    }

    public ObservableCollection<string> DatabaseNames
    {
        get => _databaseNames;
        set => this.RaiseAndSetIfChanged(ref _databaseNames, value);
    }

    public DatabaseConnection? SelectedDatabaseConnection
    {
        get => _selectedDatabaseConnection;
        set => this.RaiseAndSetIfChanged(ref _selectedDatabaseConnection, value);
    }

    public string? SelectedDatabaseName
    {
        get => _selectedDatabaseName;
        set => this.RaiseAndSetIfChanged(ref _selectedDatabaseName, value);
    }
    
    public bool ShowOnlyRM12Databases
    {
        get => _showOnlyRM12Databases;
        set => this.RaiseAndSetIfChanged(ref _showOnlyRM12Databases, value);
    }

    public bool ShowOnlyDefaultLocations
    {
        get => _showOnlyDefaultLocations;
        set => this.RaiseAndSetIfChanged(ref _showOnlyDefaultLocations, value);
    }

    public string DatabaseNameFilter
    {
        get => _databaseNameFilter;
        set => this.RaiseAndSetIfChanged(ref _databaseNameFilter, value);
    }
    
    public ICommand UpdateDataInfoCommand { get; }

    public DataInfoFileViewModel()
    {
        IDatabaseService databaseService = Locator.Current.GetRequiredService<IDatabaseService>();
        IEnumerable<DatabaseConnection> dbs = databaseService.GetDatabases();
        DatabaseConnections = new(dbs);

        this.WhenAnyValue(
                x => x.SelectedDatabaseConnection, 
                x => x.ShowOnlyRM12Databases,
                x => x.ShowOnlyDefaultLocations, 
                x => x.DatabaseNameFilter
            )
            .Subscribe(t => LoadDatabases(t.Item1, t.Item2, t.Item3, t.Item4));

        UpdateDataInfoCommand = ReactiveCommand.Create(
            UpdateDataInfo,
            this.WhenAnyValue(x => x.SelectedDatabaseName).Select(x => x != null)
        );
    }
    
    private void LoadDatabases(DatabaseConnection? connection, bool showOnlyRM12Databases, bool showOnlyDefaultLocations, string databaseNameFilter)
    {
        DatabaseNames.Clear();
        SelectedDatabaseName = null;
        if (connection == null) { return; }

        try
        {
            IEnumerable<string> databases = Enumerable.Empty<string>();
            MySqlConnectionStringBuilder builder = new()
            {
                Server = connection.Server,
                Port = 3306,
                UserID = connection.UserName,
                Password = connection.Password,
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

    private void UpdateDataInfo()
    {
        if (SelectedDatabaseConnection == null) return;
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
                    lines[i] = $"server={SelectedDatabaseConnection.Server}";
                }
                else if (lines[i].StartsWith("dbuserid="))
                {
                    lines[i] = $"dbuserid={SelectedDatabaseConnection.UserName}";
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