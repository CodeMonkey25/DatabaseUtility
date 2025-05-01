namespace DatabaseUtility.Services;

public interface IDataInfoFileService
{
    (string server, string db) GetDataInfo(string filePath);
    void SaveDataInfo(string filePath, string server, string db);
}