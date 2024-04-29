using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace InmetaTemplate.Web.IntegrationsTests.Xunit;

public class XunitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _output;

    public XunitLoggerProvider(ITestOutputHelper output)
    {
        _output = output;
    }
    
    public void Dispose () {}

    public ILogger CreateLogger(string categoryName)
    {
        return new XunitLogger(_output);
    }
}