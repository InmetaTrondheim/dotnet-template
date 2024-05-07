namespace Template._1.Web.IntegrationsTests.Infrastructure;

/// <summary>
/// Test collection for API tests.
/// For more on test collections see https://xunit.net/docs/running-tests-in-parallel.html
/// and for more on <see cref="ICollectionFixture{TFixture}"/> see https://xunit.net/docs/shared-context.html#collection-fixture.
/// </summary>
[CollectionDefinition(nameof(ApiTestCollection))]
public class ApiTestCollection : ICollectionFixture<SqlServerTestFixture>
{
    
}