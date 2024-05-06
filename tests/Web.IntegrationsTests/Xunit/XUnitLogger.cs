using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace InmetaTemplate.Web.IntegrationsTests.Xunit;
    public class XunitLogger(ITestOutputHelper output) : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var log = $"{logLevel} {formatter(state, exception)}";

            if (exception != null)
            {
                log += $"{Environment.NewLine} + Exception: {exception}";
            }

            output.WriteLine(log);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
