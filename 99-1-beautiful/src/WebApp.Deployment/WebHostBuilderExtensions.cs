using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Deployment
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder WithWebHostBuilder(
            this IWebHostBuilder builder,
            Func<IHostingEnvironment, bool> isEnvironmentSupported,
            Action<IWebHostBuilder> configure)
        {
            if (builder == null) {throw new ArgumentNullException(nameof(builder));}

            if (isEnvironmentSupported == null)
            {
                throw new ArgumentNullException(nameof(isEnvironmentSupported));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            EnvironmentAwareWebHostBuilder environmentAwareBuilder =
                builder.BeginEnvironment(isEnvironmentSupported);
            configure(environmentAwareBuilder);
            return environmentAwareBuilder.EndEnvironment();
        }
        
        public static IWebHostBuilder WithWebHostBuilder(
            this IWebHostBuilder builder,
            Func<IHostingEnvironment, bool> isEnvironmentSupported,
            Type configuratorType)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }
            if (isEnvironmentSupported == null) { throw new ArgumentNullException(nameof(isEnvironmentSupported)); }
            if (configuratorType == null) { throw new ArgumentNullException(nameof(configuratorType)); }

            Type configuratorInterfaceType = typeof(IEnvironmentSpecificWebHostConfigurator);
            if (!configuratorInterfaceType.IsAssignableFrom(configuratorType) || 
                configuratorType.IsAbstract ||
                !configuratorType.IsClass)
            {
                throw new ArgumentException(
                    $"The {configuratorType.FullName} must implement {configuratorInterfaceType.FullName}. And should be able to instantiate.");
            }

            var configurator = (IEnvironmentSpecificWebHostConfigurator)Activator.CreateInstance(configuratorType);

            return builder.WithWebHostBuilder(
                isEnvironmentSupported,
                configurator.Configure);
        }
        
        public static IWebHostBuilder WithWebHostBuilder<TConfigurator>(
            this IWebHostBuilder builder,
            Func<IHostingEnvironment, bool> isEnvironmentSupported)
            where TConfigurator : IEnvironmentSpecificWebHostConfigurator
        {
            return builder.WithWebHostBuilder(isEnvironmentSupported, typeof(TConfigurator));
        }

        public static IWebHostBuilder UseEnvironmentAwareStartup(
            this IWebHostBuilder builder,
            params (Func<IHostingEnvironment, bool> isEnvironmentSupported, Func<IServiceProvider, IEnvironmentSpecificStartup> environmentStartupFactory)[] startupConfigs)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }
            ValidateStartupConfigs(startupConfigs);

            builder.ConfigureServices((c, s) =>
            {
                foreach (var startupConfig in startupConfigs)
                {
                    if (startupConfig.isEnvironmentSupported(c.HostingEnvironment))
                    {
                        s.AddSingleton(startupConfig.environmentStartupFactory);
                    }
                }
            }).UseStartup<EnvironmentAwareStartup>();
            
            return builder;
        }
        
        public static IWebHostBuilder UseEnvironmentAwareStartup(
            this IWebHostBuilder builder,
            params (Func<IHostingEnvironment, bool> isEnvironmentSupported, Type environmentStartupType)[] startupConfigs)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }
            ValidateStartupConfigs(startupConfigs);

            builder.ConfigureServices(
                (c, s) =>
                {
                    foreach (var startupConfig in startupConfigs)
                    {
                        if (startupConfig.isEnvironmentSupported(c.HostingEnvironment))
                        {
                            s.AddSingleton(typeof(IEnvironmentSpecificStartup), startupConfig.environmentStartupType);
                            return;
                        }
                    }
                }).UseStartup<EnvironmentAwareStartup>();

            return builder;
        }

        static void ValidateStartupConfigs(
            (Func<IHostingEnvironment, bool> isEnvironmentSupported, Type environmentStartupType)[] startupConfigs)
        {
            if (startupConfigs == null) { throw new ArgumentNullException(nameof(startupConfigs)); }

            if (startupConfigs.Any(startupConfig =>
                startupConfig.isEnvironmentSupported == null || startupConfig.environmentStartupType == null))
            {
                throw new ArgumentException(
                    $"The environment specific startup must provide environment prediction and startup type.");
            }
        }

        static void ValidateStartupConfigs(
            (
                Func<IHostingEnvironment, bool> isEnvironmentSupported,
                Func<IServiceProvider, IEnvironmentSpecificStartup> environmentStartupFactory
            )[] startupConfigs)
        {
            if (startupConfigs == null) { throw new ArgumentNullException(nameof(startupConfigs)); }

            if (startupConfigs.Any(startupConfig =>
                startupConfig.isEnvironmentSupported == null || startupConfig.environmentStartupFactory == null))
            {
                throw new ArgumentException(
                    $"The environment specific startup must provide environment prediction and factory method.");
            }
        }

        static EnvironmentAwareWebHostBuilder BeginEnvironment(
            this IWebHostBuilder builder,
            Func<IHostingEnvironment, bool> isEnvironmentSupported)
        {
            if (builder == null) {throw new ArgumentNullException(nameof(builder));}

            if (isEnvironmentSupported == null)
            {
                throw new ArgumentNullException(nameof(isEnvironmentSupported));
            }

            return new DelegatedWebHostBuilder(builder, isEnvironmentSupported);
        }

        static IWebHostBuilder EndEnvironment(
            this IWebHostBuilder webHostBuilder)
        {
            EnvironmentAwareWebHostBuilder environmentAwareWebHostBuilder =
                ToEnvironmentAwareWebHostBuilder(webHostBuilder);
            return environmentAwareWebHostBuilder.UnderlyingBuilder;
        }

        static EnvironmentAwareWebHostBuilder ToEnvironmentAwareWebHostBuilder(
            IWebHostBuilder webHostBuilder)
        {
            var environmentAwareWebHostBuilder = webHostBuilder as EnvironmentAwareWebHostBuilder;

            if (environmentAwareWebHostBuilder == null)
            {
                throw new ArgumentException(
                    $"The {nameof(IWebHostBuilder)} is not environment aware.");
            }

            return environmentAwareWebHostBuilder;
        }
    }
}