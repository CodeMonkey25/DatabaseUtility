using System;
using ReactiveUI;
using Splat;

namespace DatabaseUtility.Services;

public class LoggerService : ReactiveObject, ILoggerService
{
    public void LogInfo(string message)
    {
        this.Log().Info(message);
    }

    public void LogError(string message)
    {
        this.Log().Error(message);
    }

    public void LogError(Exception ex, string message)
    {
        this.Log().Error(ex, message);
    }
}