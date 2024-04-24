using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Web.Helpers;

namespace Web.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = configuration["Authentication:Authority"];
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "roles",
                    ValidateAudience = false
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicies.RequireApiScope, builder =>
                builder.RequireAuthenticatedUser()
                    .RequireApiScope());
        });

        return services;
    }

    private const string ApiScope = "api";

    private static void RequireApiScope(this AuthorizationPolicyBuilder builder)
    {
        builder.RequireClaim("scope", ApiScope);
    }
}