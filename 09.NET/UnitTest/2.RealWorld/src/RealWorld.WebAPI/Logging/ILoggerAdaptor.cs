namespace RealWorld.WebAPI.Logging;

public interface ILoggerAdaptor<TType>
{
    void LogInformation(string? message, params object?[] args);
    void LogError(Exception? exception, string? message, params object?[] args);
}

public sealed class LoggerAdaptor<TType>(
    ILogger<TType> logger) : ILoggerAdaptor<TType>
{
    public void LogError(Exception? exception, string? message, params object?[] args)
    {
        logger.LogError(exception, message, args);
    }

    public void LogInformation(string? message, params object?[] args)
    {
        logger.LogInformation(message, args);
    }
}