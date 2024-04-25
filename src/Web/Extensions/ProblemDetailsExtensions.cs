using Domain.ErrorHandling;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace Web.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetails(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddProblemDetails(o =>
        {
            o.IncludeExceptionDetails = (_, _) => env.IsDevelopment();

            o.MapFluentValidationException();

            o.Map<ApiException>(ex => ex.AddDetail());
        });

        return services;
    }

    private static ProblemDetails AddDetail<T>(this T ex) where T : ApiException => new ApiErrorProblemDetails(ex.ApiError);

    public static void MapFluentValidationException(this ProblemDetailsOptions options) =>
        options.Map<ValidationException>((ctx, ex) =>
        {
            var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            var errors = ex.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(f => f.ErrorMessage).ToArray());

            return factory.CreateValidationProblemDetails(ctx, errors);
        });
}

public class ApiErrorProblemDetails : StatusCodeProblemDetails
{
    public string ErrorCode { get; set; }

    public ApiErrorProblemDetails(ApiError apiError) : base(apiError.StatusCode)
    {
        ErrorCode = apiError.ErrorCode;
        Detail = apiError.Message;
    }
}