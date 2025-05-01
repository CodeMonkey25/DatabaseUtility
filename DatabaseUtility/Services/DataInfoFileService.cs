using System.IO;

namespace DatabaseUtility.Services;

public class DataInfoFileService : IDataInfoFileService
{
    public (string server, string db) GetDataInfo(string filePath)
    {
        string server = string.Empty;
        string db = string.Empty;
        
        foreach (string line in File.ReadLines(filePath))
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
        
        return (server, db);
    }

    public void SaveDataInfo(string filePath, string server, string db)
    {
        string[] lines = File.ReadAllLines(filePath);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("database="))
            {
                lines[i] = $"database={db[..^5]}";
            }
            else if (lines[i].StartsWith("server="))
            {
                lines[i] = $"server={server}";
            }
        }

        File.WriteAllLines(filePath, lines);
    }
}