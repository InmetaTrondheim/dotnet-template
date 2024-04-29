namespace InmetaTemplate.Web.Middlewares;

public class UnhandledExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UnhandledExceptionLoggingMiddleware> _logger;

    public UnhandledExceptionLoggingMiddleware(RequestDelegate next, ILogger<UnhandledExceptionLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An error occured while processing {method} {path}",
                context.Request.Method, context.Request.Path
            );

            throw;
        }
    }
}