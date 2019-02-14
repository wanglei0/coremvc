using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace WebApp.Logging
{
    static class WebAppLogger
    {
        public static ILoggingBuilder AddWebAppLogger(
            this ILoggingBuilder loggingBuilder,
            WebHostBuilderContext context)
        {
            Logger logger = CreateLogger(context);
            Log.Logger = logger;
            return loggingBuilder.AddSerilog(logger, true);
        }

        static Logger CreateLogger(WebHostBuilderContext context)
        {
            // Please pay attention that the appSettings.json will always have the highest priority.
            // So it is better define logging configuration in separate setting files. When you
            // create settings files, be sure that the build action of the file is "Content" or it
            // will not be applied.
            
            return new LoggerConfiguration()
                .ReadFrom.Configuration(context.Configuration)
                .CreateLogger();
        }
    }
}