using System;
using System.Reactive.Linq;
using DatabaseUtility.Models;
using ReactiveUI;

namespace DatabaseUtility.ViewModels;

public class EditDatabaseViewModel : ViewModelBase
{
    private DatabaseConnection OriginalDatabaseConnection { get; }

    private DatabaseConnection _workingDatabaseConnection = new();
    private IObservable<bool> _canSaveChanges = Observable.Empty<bool>();
    
    public DatabaseConnection WorkingDatabaseConnection
    {
        get => _workingDatabaseConnection;
        set => this.RaiseAndSetIfChanged(ref _workingDatabaseConnection, value);
    }
    
    public IObservable<bool> CanSaveChanges
    {
        get => _canSaveChanges;
        set => this.RaiseAndSetIfChanged(ref _canSaveChanges, value);
    }

    public EditDatabaseViewModel(DatabaseConnection databaseConnection)
    {
        OriginalDatabaseConnection = databaseConnection;
        WorkingDatabaseConnection.OverwriteWith(databaseConnection);

        CanSaveChanges = WorkingDatabaseConnection
            .WhenAnyValue(x => x.Name, x => x.Server, x => x.UserName, x => x.Password)
            .Select(x => !string.IsNullOrEmpty(x.Item1) && !string.IsNullOrEmpty(x.Item2) && !string.IsNullOrEmpty(x.Item3) && !string.IsNullOrEmpty(x.Item4));
    }

    public void SaveChanges()
    {
        OriginalDatabaseConnection.OverwriteWith(WorkingDatabaseConnection);
    }
}