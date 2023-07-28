using System.Collections.Generic;
using System.Collections.ObjectModel;
using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.Extensions;
using ReactiveUI;
using Splat;

namespace DatabaseUtility.ViewModels;

public class ListDatabaseViewModel : ViewModelBase
{
    private ObservableCollection<DatabaseConnection> _databases = new();
    public ObservableCollection<DatabaseConnection> Databases
    {
        get => _databases;
        set => this.RaiseAndSetIfChanged(ref _databases, value);
    }

    public ListDatabaseViewModel()
    {
        IDatabaseService databaseService = Locator.Current.GetRequiredService<IDatabaseService>();
        IEnumerable<DatabaseConnection> dbs = databaseService.GetDatabases();
        Databases = new(dbs);
    }

    public void SaveChanges()
    {
        IDatabaseService databaseService = Locator.Current.GetRequiredService<IDatabaseService>();
        databaseService.SaveDatabases(Databases);    
    }

    public void AddDatabase(DatabaseConnection db)
    {
        Databases.Add(db);
        IDatabaseService databaseService = Locator.Current.GetRequiredService<IDatabaseService>();
        databaseService.SaveDatabases(Databases);    
    }
}