using System.Collections.Generic;

namespace DatabaseUtility.Services;

public interface IDatabaseService
{
    List<string> GetDatabaseNames(string server);
}