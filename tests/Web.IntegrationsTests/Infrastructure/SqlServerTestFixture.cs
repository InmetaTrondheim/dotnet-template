using Testcontainers.MsSql;

namespace InmetaTemplate.Web.IntegrationsTests.Infrastructure;

public class SqlServerTestFixture: IAsyncLifetime
{
    private MsSqlContainer _mssqlContainer;
    private const string MssqlImg = "mcr.microsoft.com/mssql/server:2019-latest";
    public string ConnectionString { get; private set; }

    public async Task InitializeAsync()
    {
        try
        {
            _mssqlContainer = new MsSqlBuilder()
                .WithImage(MssqlImg)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithCleanUp(true)
                .Build();

            await _mssqlContainer.StartAsync();
            ConnectionString = _mssqlContainer.GetConnectionString();
        }
        catch (Exception ex)
        {
            throw new Exception("Do you have Docker running?", ex);
        }
    }

    public async Task DisposeAsync()
    {
        await _mssqlContainer.DisposeAsync();
    }
}