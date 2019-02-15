using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace WebApp.Logging
{
    static class WebAppLogger
    {
        /// <summary>
        /// This method will create a temporary logger. It is used before the web host is created
        /// when no logger is available. Please do remember to close the logger after the web host
        /// has been initialized.
        /// </summary>
        /// <returns>A temporary logger that record logs before application initialization.</returns>
        public static IEmergencyLogger CreateEmergencyLogger()
        {
            return new SerilogEmergencyLogger(
                new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger());
        }

        public static IWebHostBuilder UseWebAppLogger(this IWebHostBuilder builder)
        {
            // Please pay attention that the appSettings.json will always have the highest priority.
            // So it is better define logging configuration in separate setting files. When you
            // create settings files, be sure that the build action of the file is "Content" or it
            // will not be applied.
            
            return builder.UseSerilog(
                (ctx, logging) => logging.ReadFrom.Configuration(ctx.Configuration));
        }
    }
}