using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Web.IntegrationsTests.Infrastructure;

public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemaName = "Test";

    public AuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Integration Tests User"),
            new Claim("sub", "1"),
            new Claim("scope", "api"),
        };

        var identity = new ClaimsIdentity(claims, SchemaName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemaName);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}