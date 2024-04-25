using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Filters;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    private string Scope { get; }

    public AuthorizeCheckOperationFilter(string scope)
    {
        Scope = scope;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var showAuthorize = CheckShowAuthorize(context);

        if (!showAuthorize) return;

        if (!operation.Responses.ContainsKey("401"))
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });

        if (!operation.Responses.ContainsKey("403"))
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                [
                    new OpenApiSecurityScheme {Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"}
                    }
                ] = new[] { Scope }
            }
        };
    }

    private bool CheckShowAuthorize(OperationFilterContext context)
    {
        return !HasAllowAnonymousAttribute(context);
    }

    private static bool HasAllowAnonymousAttribute(OperationFilterContext context)
    {
        return context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() ||
               context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
    }
}