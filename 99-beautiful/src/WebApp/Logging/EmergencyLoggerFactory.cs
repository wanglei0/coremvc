using System;
using Serilog;
using Serilog.Core;

namespace WebApp.Logging
{
    static class EmergencyLoggerFactory
    {
        /// <summary>
        /// This method will create a temporary logger. It is used before the web host is created
        /// when no logger is available. Please do remember to close the logger after the web host
        /// has been initialized.
        /// </summary>
        /// <returns>A temporary logger that record logs before application initialization.</returns>
        public static IEmergencyLogger Create()
        {
            return new SerilogEmergencyLogger(
                new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger());
        }
        
        class SerilogEmergencyLogger : IEmergencyLogger
        {
            readonly Logger logger;

            public SerilogEmergencyLogger(Logger logger) { this.logger = logger; }
        
            public void Dispose() { logger.Dispose(); }

            public void Fatal(Exception exception, string messageTemplate, params object[] args)
            {
                logger.Fatal(exception, messageTemplate, args);
            }

            public void Warning(string messageTemplate, params object[] args)
            {
                logger.Warning(messageTemplate, args);
            }
        }
    }
}