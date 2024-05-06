using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace InmetaTemplate.Web.IntegrationsTests.Xunit;

public class XunitLoggerProvider(ITestOutputHelper output) : ILoggerProvider
{
    public void Dispose () {}

    public ILogger CreateLogger(string categoryName)
    {
        return new XunitLogger(output);
    }
}