using System;

namespace DatabaseUtility.Services;

public interface ILoggerService
{
    void LogInfo(string message);
    void LogError(string message);
    void LogError(Exception ex, string message);
}