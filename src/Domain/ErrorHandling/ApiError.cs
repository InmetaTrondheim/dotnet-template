namespace InmetaTemplate.Domain.ErrorHandling;

public class ApiError
{
    public int StatusCode { get; }
    public string ErrorCode { get; }
    public string Message { get; }

    private ApiError(int statusCode, string errorCode, string message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        Message = message;
    }

    protected internal static ApiError InternalServerError(string errorCode, string message) =>
        new(500, errorCode, message);


    internal static ApiError Forbidden(string errorCode, string message) =>
        new(403, errorCode, message);


    protected internal static ApiError BadRequest(string errorCode, string message) =>
        new(400, errorCode, message);

    protected internal static ApiError NotFound(string errorCode, string message) =>
        new(404, errorCode, message);
}