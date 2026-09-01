namespace ZMT.SteelCost.Application.Logging;

public interface IAppLogger
{
    void Information(string eventName, string message, object? data = null);
    void Error(string eventName, Exception exception, object? data = null);
}
