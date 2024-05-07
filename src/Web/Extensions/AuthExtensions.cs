using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Template._1.Web.Helpers;

namespace Template._1.Web.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = configuration["Authentication:Authority"];
                options.Audience = configuration["Authentication:Audience"];
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "roles",
                    ValidateAudience = !string.IsNullOrEmpty(configuration["Authentication:Audience"])
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicies.RequireApiScope, builder =>
                builder.RequireAuthenticatedUser()
                    .RequireApiScope(configuration));
        });

        return services;
    }

    private static void RequireApiScope(this AuthorizationPolicyBuilder builder, IConfiguration configuration)
    {
        var scopeClaimType = configuration["Authentication:ScopeClaimType"];
        var scopeClaimValue = configuration["Authentication:ScopeClaimValue"];

        if (!string.IsNullOrEmpty(scopeClaimType) && !string.IsNullOrEmpty(scopeClaimValue))
        {
            builder.RequireClaim(scopeClaimType, scopeClaimValue);
        }
    }
}