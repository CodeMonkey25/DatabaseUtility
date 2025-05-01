using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.Utility;
using DynamicData;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;

namespace DatabaseUtility.ViewModels;

public partial class DataInfoViewModel : ViewModelBase
{
    public override ViewTypes ViewType => ViewTypes.DataInfo;

    private readonly IDatabaseService? _databaseService;
    private readonly ILoggerService? _loggerService;
    private readonly IObservable<bool>? _canUpdateDataInfoExecute;
    
    [Reactive] private string _dataInfoFilePath = @"C:\Users\jsarley\source\repos\rm\UI\Desktop\WPFClient\bin\Debug\datainfo.dat";
    [Reactive] private ObservableCollection<string> _databaseServers = [];
    [Reactive] private ObservableCollection<string> _databaseNames = [];
    [Reactive] private string? _selectedDatabaseServer;
    [Reactive] private string? _selectedDatabaseName;
    
    public DataInfoViewModel() { /* constructor for axaml designer */ }
    
    [DependencyInjectionConstructor]
    public DataInfoViewModel(ISettingsService? settingsService, IDatabaseService? databaseService, ILoggerService? loggerService)
    {
        ISettingsService settingsService1 = settingsService ?? throw new Exception("settings service is required");
        _databaseService = databaseService ?? throw new Exception("database service is required");
        _loggerService = loggerService ?? throw new Exception("logger service is required");
        Settings settings = settingsService1.Get();
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
            _loggerService?.LogError(e, "Unable to load datainfo file");
        }
    }
    
    private void LoadDatabases(string? serverName)
    {
        DatabaseNames.Clear();
        SelectedDatabaseName = null;
        if (serverName == null) { return; }
        
        try
        {
            DatabaseNames.AddRange(_databaseService!.GetDatabaseNames(serverName));
        }
        catch (Exception e)
        {
            _loggerService?.LogError(e, "Unable to load database names");
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
            _loggerService?.LogError(e, "Unable to update datainfo file");
        }
    }
}