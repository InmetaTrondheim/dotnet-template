namespace InmetaTemplate.Web.Middlewares;

public class UnhandledExceptionLoggingMiddleware(
    RequestDelegate next,
    ILogger<UnhandledExceptionLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occured while processing {method} {path}",
                context.Request.Method, context.Request.Path
            );

            throw;
        }
    }
}