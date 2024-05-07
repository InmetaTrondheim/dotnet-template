namespace Template._1.Web.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        var start = DateTime.UtcNow;

        logger.LogInformation("Processing request: {method} {path}",
            context.Request.Method, context.Request.Path);

        try
        {
            await next(context);
        }
        finally
        {
            var duration = DateTime.UtcNow - start;

            logger.LogInformation(
                "Finished processing request: {method} {path}. Responded in {duration}ms",
                context.Request.Method,
                context.Request.Path,
                duration.TotalMilliseconds
            );
        }
    }
}