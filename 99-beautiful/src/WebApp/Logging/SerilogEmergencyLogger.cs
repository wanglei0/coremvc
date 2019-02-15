using System;
using Serilog.Core;

namespace WebApp.Logging
{
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