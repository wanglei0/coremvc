using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace WebApp.Logging
{
    static class LoggingServiceCollectionExtensions
    {
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