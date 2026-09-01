namespace ZMT.SteelCost.Infrastructure.Persistence;

public static class AppPaths
{
    public static string DatabasePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ZMT", "SteelCost", "steelcost.db");

    public static string LogDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ZMT", "SteelCost", "Logs");

    public static string BackupDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ZMT", "SteelCost", "Backups");
}
