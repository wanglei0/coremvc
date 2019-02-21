using Microsoft.Extensions.Logging;

namespace WebApp.TestBase
{
    public class TestLoggingConfiguration
    {
        public TestLoggingConfiguration(bool enableLogging, bool supportScope, LogLevel minimumLevel)
        {
            EnableLogging = enableLogging;
            SupportScope = supportScope;
            MinimumLevel = minimumLevel;
        }

        public bool EnableLogging { get; }
        public bool SupportScope { get; }
        public LogLevel MinimumLevel { get; }

        public static TestLoggingConfiguration EnableLoggingDebug { get; } = 
            new TestLoggingConfiguration(true, true, LogLevel.Debug);
        
        public static TestLoggingConfiguration EnableLoggingWarning { get; } =
            new TestLoggingConfiguration(true, true, LogLevel.Warning);
    }
}