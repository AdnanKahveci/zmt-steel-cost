using System.Text.Json;
using ZMT.SteelCost.Application.Logging;
using ZMT.SteelCost.Infrastructure.Persistence;

namespace ZMT.SteelCost.Infrastructure.Logging;

public sealed class JsonFileLogger : IAppLogger
{
    private static readonly object Gate = new();
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public void Information(string eventName, string message, object? data = null) =>
        Write("Information", eventName, message, null, data);

    public void Error(string eventName, Exception exception, object? data = null) =>
        Write("Error", eventName, exception.Message, exception.ToString(), data);

    private static void Write(string level, string eventName, string message, string? exception, object? data)
    {
        Directory.CreateDirectory(AppPaths.LogDirectory);
        var path = Path.Combine(AppPaths.LogDirectory, $"steelcost-{DateTime.Today:yyyyMMdd}.jsonl");
        var entry = new
        {
            timestamp = DateTimeOffset.Now,
            level,
            eventName,
            message,
            exception,
            data
        };
        var line = JsonSerializer.Serialize(entry, Options) + Environment.NewLine;
        lock (Gate)
        {
            File.AppendAllText(path, line);
        }
    }
}
