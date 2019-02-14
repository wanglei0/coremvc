using System;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WebApp.Logging
{
    static class WebAppLogger
    {
        public static void Initialize()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.EventCollector(
                    splunkHost: "http://127.0.0.1:4445",
                    eventCollectorToken: "5606bef3-1a9f-4aae-a190-b8f3d7503ad7")
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}