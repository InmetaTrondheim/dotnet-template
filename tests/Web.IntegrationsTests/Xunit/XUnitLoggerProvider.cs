using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Template._1.Web.IntegrationsTests.Xunit;

public class XunitLoggerProvider(ITestOutputHelper output) : ILoggerProvider
{
    public void Dispose () {}

    public ILogger CreateLogger(string categoryName)
    {
        return new XunitLogger(output);
    }
}