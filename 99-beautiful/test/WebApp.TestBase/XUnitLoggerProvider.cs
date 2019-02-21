using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace WebApp.TestBase
{
    class XUnitLoggerProvider : ILoggerProvider
    {
        readonly ITestOutputHelper output;
        readonly bool supportScope;

        public XUnitLoggerProvider(ITestOutputHelper output, bool supportScope)
        {
            this.output = output;
            this.supportScope = supportScope;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(
                categoryName,
                output,
                supportScope ? new LoggerExternalScopeProvider() : null);
        }

        public void Dispose() { }
    }
}