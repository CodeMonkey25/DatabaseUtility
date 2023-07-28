using DatabaseUtility.Models;
using ReactiveUI;

namespace DatabaseUtility.ViewModels;

public class EditDatabaseViewModel : ViewModelBase
{
    private DatabaseConnection OriginalDatabaseConnection { get; }

    private DatabaseConnection _workingDatabaseConnection = new();
    public DatabaseConnection WorkingDatabaseConnection
    {
        get => _workingDatabaseConnection;
        set => this.RaiseAndSetIfChanged(ref _workingDatabaseConnection, value);
    }

    public EditDatabaseViewModel(DatabaseConnection databaseConnection)
    {
        OriginalDatabaseConnection = databaseConnection;
        WorkingDatabaseConnection.OverwriteWith(databaseConnection);
    }

    public void SaveChanges()
    {
        OriginalDatabaseConnection.OverwriteWith(WorkingDatabaseConnection);
    }
}