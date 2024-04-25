using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Web.Filters;

namespace Web.Extensions;

public static class SwaggerExtensions
{
    public static void AddOauth(this SwaggerGenOptions opt, IConfiguration configuration)
    {
        opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"{configuration["Authentication:Authority"]}/connect/authorize"),
                    TokenUrl = new Uri($"{configuration["Authentication:Authority"]}/connect/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        {configuration["Authentication:ScopeClaimValue"] ?? string.Empty, "api scope"},
                    }
                }
            }
        });

        opt.OperationFilter<AuthorizeCheckOperationFilter>(configuration["Authentication:ScopeClaimValue"] ?? string.Empty);
    }

    public static void UseOauth(this SwaggerUIOptions opt, IConfiguration configuration)
    {
        opt.OAuthClientId(configuration["Authentication:ClientId"]);
        opt.OAuthScopes(configuration["Authentication:ScopeClaimValue"] ?? string.Empty);
        opt.OAuthClientSecret(configuration["Authentication:ClientSecret"]);
        opt.OAuthUsePkce();
        opt.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    }
}