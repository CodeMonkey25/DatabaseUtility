using System.Collections.Generic;
using DatabaseUtility.Models;

namespace DatabaseUtility.Services;

public interface IDatabaseService
{
    IEnumerable<DatabaseConnection> GetDatabases();
    void SaveDatabases(IEnumerable<DatabaseConnection> databases);
}