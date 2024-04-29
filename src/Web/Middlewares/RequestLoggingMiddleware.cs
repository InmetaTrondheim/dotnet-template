namespace InmetaTemplate.Web.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var start = DateTime.UtcNow;

        _logger.LogInformation("Processing request: {method} {path}",
            context.Request.Method, context.Request.Path);

        try
        {
            await _next(context);
        }
        finally
        {
            var duration = DateTime.UtcNow - start;

            _logger.LogInformation(
                "Finished processing request: {method} {path}. Responded in {duration}ms",
                context.Request.Method,
                context.Request.Path,
                duration.TotalMilliseconds
            );
        }
    }
}