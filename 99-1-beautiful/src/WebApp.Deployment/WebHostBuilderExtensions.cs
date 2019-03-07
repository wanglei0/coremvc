using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Deployment.Initialization;

namespace WebApp.Deployment
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureEnvironment(
            this IWebHostBuilder builder,
            params (string environmentName, IEnvironmentSetup config)[] configs)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }
            if (configs == null) { throw new ArgumentNullException(nameof(configs)); }

            if (configs.Any(cfg => cfg.config == null || string.IsNullOrEmpty(cfg.environmentName)))
            {
                throw new ArgumentException("The environment name and configuration are mandatory.");
            }

            if (configs.Select(cfg => cfg.environmentName).ContainsDuplication(StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("The environment name contains duplication");
            }

            IEnumerable<(string, Action<IWebHostBuilder>, Type)> delegateBasedConfigs = configs.Select(
                cfg =>
                {
                    var configurator = (IWebHostConfiguratorForEnvironment) Activator.CreateInstance(
                        cfg.config.ConfiguratorType);
                    return (
                        cfg.environmentName,
                        (Action<IWebHostBuilder>) configurator.Configure,
                        cfg.config.StartupType
                    );
                });

            return ConfigureEnvironmentCore(builder, delegateBasedConfigs.ToArray());
        }

        public static IWebHostBuilder ConfigureEnvironment(
            this IWebHostBuilder builder,
            params (string environmentName, Action<IWebHostBuilder> configureBuilder, Type startupType)[] configs)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }
            if (configs == null) { throw new ArgumentNullException(nameof(configs)); }

            if (configs.Any(
                cfg => cfg.configureBuilder == null || cfg.startupType == null ||
                       string.IsNullOrEmpty(cfg.environmentName)))
            {
                throw new ArgumentException("The environment name and configuration are mandatory.");
            }

            if (configs.Select(cfg => cfg.environmentName).ContainsDuplication(StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("The environment name contains duplication");
            }
            
            return ConfigureEnvironmentCore(builder, configs);
        }

        static IWebHostBuilder ConfigureEnvironmentCore(
            IWebHostBuilder builder,
            (string environmentName, Action<IWebHostBuilder> configureBuilder, Type startupType)[] configs)
        {
            foreach ((string environmentName, Action<IWebHostBuilder> configureBuilder, Type _) config in configs)
            {
                bool IsEnvironmentSupported(IHostingEnvironment h) => h.IsEnvironment(config.environmentName);
                
                EnvironmentAwareWebHostBuilder environmentAwareBuilder =
                    new DelegatedWebHostBuilder(builder, IsEnvironmentSupported);
                config.configureBuilder(environmentAwareBuilder);
            }

            return builder.UseEnvironmentAwareStartup(
                configs.Select(cfg => (cfg.environmentName, cfg.startupType)).ToArray());
        }

        static IWebHostBuilder UseEnvironmentAwareStartup(
            this IWebHostBuilder builder,
            params (string environmentName, Type environmentStartupType)[] startupConfigs)
        {
            builder.ConfigureServices(
                (c, s) =>
                {
                    Type startupType = startupConfigs
                        .Where(cfg => c.HostingEnvironment.IsEnvironment(cfg.environmentName))
                        .Select(cfg => cfg.environmentStartupType)
                        .SingleOrDefault();
                    if (startupType == null) { return; }

                    s.AddSingleton(typeof(IStartupForEnvironment), startupType);
                }).UseStartup<EnvironmentAwareStartup>();

            return builder;
        }
    }
}