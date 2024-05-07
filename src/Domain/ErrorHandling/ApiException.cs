namespace Template._1.Domain.ErrorHandling;

/// <summary>
/// Api exception to throw when logic error occurs and we want to signal that to the front end with a specified <see cref="ApiError"/>
/// See <see cref="CommonErrors"/> for example errors
/// </summary>
public class ApiException : Exception
{
    public ApiError ApiError { get; set; }

    public ApiException() : base(CommonErrors.General().Message)
    {
        ApiError = CommonErrors.General();
    }

    public ApiException(ApiError apiError) : base(apiError.Message)
    {
        ApiError = apiError;
    }
}