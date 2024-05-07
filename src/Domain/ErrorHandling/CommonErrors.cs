namespace Template._1.Domain.ErrorHandling;

/// <summary>
/// 
/// </summary>
public class CommonErrors
{
    public static ApiError General(string message = "Not specified") => ApiError.InternalServerError(
        "C0", $"General Api Error: {message}");

    public static ApiError EntityNotFound<T>(int id) => ApiError.NotFound("C1", $"Entity of type {typeof(T)} with id {id} not found");

    public static ApiError ForbiddenAction => ApiError.Forbidden(
        "C2", "Current user is not allowed to execute or access this resource");

    public static ApiError InvalidArguments(string message) => ApiError.BadRequest(
        "C3", $"Invalid arguments: {message}");

    public static ApiError InvalidOperation(string message) => ApiError.BadRequest(
        "C4", $"Invalid operation: {message}");
}