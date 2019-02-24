using System;
using Microsoft.AspNetCore.Hosting;

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