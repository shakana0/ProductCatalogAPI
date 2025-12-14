using Microsoft.Extensions.Logging;
public static class LoggerExtensions
{
    public static void LogApiRequest(
        this ILogger logger,
        string endpoint,
        string query,
        int statusCode,
        long responseTimeMs,
        string cacheStatus,
        string subscription,
        int resultCount)
    {
        var logEntry = new
        {
            timestamp = DateTime.UtcNow,
            endpoint,
            query,
            status = statusCode,
            responseTimeMs,
            cache = cacheStatus,
            subscription,
            resultCount
        };

        logger.LogInformation("API Request {@LogEntry}", logEntry);
    }
}
