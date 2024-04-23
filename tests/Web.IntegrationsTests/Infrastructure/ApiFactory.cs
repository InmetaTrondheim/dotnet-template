using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Web.IntegrationsTests.Xunit;
using Xunit.Abstractions;

namespace Web.IntegrationsTests.Infrastructure;

internal class ApiFactory: WebApplicationFactory<Program>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Dictionary<string, string> _overriddenAppSettings;

    internal ApiFactory(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _overriddenAppSettings = new Dictionary<string, string>();
    }

    internal Action<IServiceCollection> ConfigureTestServices { get; set; }

    internal Dictionary<string, string> OverriddenAppsettings => _overriddenAppSettings;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(OverriddenAppsettings);
        });
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ILoggerFactory>();

            services.AddAuthentication(AuthHandler.SchemaName)
                .AddScheme<AuthenticationSchemeOptions, AuthHandler>(
                    AuthHandler.SchemaName, options => { });
        });
        builder.ConfigureTestServices(services =>
        {
            ConfigureTestServices?.Invoke(services);
        });
        builder.ConfigureLogging(logBuilder =>
        {
            logBuilder
                .SetMinimumLevel(LogLevel.Information)
                .ClearProviders()
                .AddProvider(new XunitLoggerProvider(_testOutputHelper));
        });
    }
}

