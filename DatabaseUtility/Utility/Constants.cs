using System;
using System.IO;

namespace DatabaseUtility.Utility;

public static class Constants
{
    // Environment.SpecialFolder.CommonApplicationData => C:\ProgramData
    // Environment.SpecialFolder.ApplicationData => C:\Users\jsarley\AppData\Roaming
    // Environment.SpecialFolder.LocalApplicationData => C:\Users\jsarley\AppData\Local => ~/.local/share/

    internal static string LocalApplicationDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    internal static string JmsDataPath => Path.Combine(LocalApplicationDataPath, "JMS");
    internal static string DataPath => Path.Combine(JmsDataPath, "DatabaseUtility");
    internal static string LogFile => Path.Combine(DataPath, "Logs", "daily_.log");
}