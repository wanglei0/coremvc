using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Core;

namespace WebApp.Deployment
{
    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilog(
            this IServiceCollection services,
            Action<LoggerConfiguration> configure)
        {
            var loggerConfiguration = new LoggerConfiguration();
            configure(loggerConfiguration);
            Logger logger = loggerConfiguration.CreateLogger();
            Log.Logger = logger;
            services.AddSingleton<ILoggerFactory>(_ => new SerilogLoggerFactory(null, true));
            return services;
        }
    }
}