using System;
using System.Collections.ObjectModel;
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
    public const string DefaultDataInfoFilePath = @"C:\Users\jsarley\source\repos\rm\UI\Desktop\WPFClient\bin\Debug\datainfo.dat";
    public override ViewTypes ViewType => ViewTypes.DataInfo;

    private readonly ILoggerService? _loggerService;
    private readonly IDatabaseService? _databaseService;
    private readonly IDataInfoFileService? _dataInfoFileService;
    private readonly IObservable<bool>? _canUpdateDataInfoExecute;
    
    [Reactive] private string _dataInfoFilePath = DefaultDataInfoFilePath;
    [Reactive] private ObservableCollection<string> _databaseServers = [];
    [Reactive] private ObservableCollection<string> _databaseNames = [];
    [Reactive] private string? _selectedDatabaseServer;
    [Reactive] private string? _selectedDatabaseName;
    
    public DataInfoViewModel() { /* constructor for axaml designer */ }
    
    [DependencyInjectionConstructor]
    public DataInfoViewModel(ILoggerService? loggerService, ISettingsService? settingsService, IDatabaseService? databaseService, IDataInfoFileService? dataInfoFileService)
    {
        _loggerService = loggerService ?? throw new Exception("logger service is required");
        ISettingsService settingsService1 = settingsService ?? throw new Exception("settings service is required");
        _databaseService = databaseService ?? throw new Exception("database service is required");
        _dataInfoFileService = dataInfoFileService ?? throw new Exception("datainfo file service is required");
        
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
            (string server, string db) = _dataInfoFileService!.GetDataInfo(DataInfoFilePath);
            
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
            _dataInfoFileService!.SaveDataInfo(DataInfoFilePath, SelectedDatabaseServer, SelectedDatabaseName);
        }
        catch (Exception e)
        {
            _loggerService?.LogError(e, "Unable to update datainfo file");
        }
    }
}