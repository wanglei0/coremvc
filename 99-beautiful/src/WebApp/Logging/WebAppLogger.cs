using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace WebApp.Logging
{
    static class WebAppLogger
    {
        /// <summary>
        /// This method will create a temporary logger from a configuration file. It is used before
        /// the web host is created when no logger is available. So please do remember to close the
        /// logger after the web host has been initialized.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>A temporary logger that record logs before application initialization.</returns>
        public static IEmergencyLogger CreateEmergencyLogger(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            if (args != null)
            {
                configurationBuilder.AddCommandLine(args);
            }

            IConfiguration configuration = configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    true,
                    false)
                .AddEnvironmentVariables()
                .Build();

            return new SerilogEmergencyLogger(
                new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
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