using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Deployment
{
    abstract class EnvironmentAwareWebHostBuilder : IWebHostBuilder
    {
        public IWebHostBuilder UnderlyingBuilder { get; }

        protected abstract bool IsSupported(IHostingEnvironment hostingEnvironment);

        protected EnvironmentAwareWebHostBuilder(IWebHostBuilder underlyingBuilder)
        {
            if (underlyingBuilder is EnvironmentAwareWebHostBuilder)
            {
                throw new InvalidOperationException(
                    "Cannot create nested environment aware WebHostBuilder.");
            }
            
            UnderlyingBuilder = underlyingBuilder;
        }

        IWebHost IWebHostBuilder.Build()
        {
            throw new InvalidOperationException(
                "Cannot call Build() in an environment aware block.");
        }

        public IWebHostBuilder ConfigureAppConfiguration(
            Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            UnderlyingBuilder.ConfigureAppConfiguration(
                (context, configBuilder) =>
                {
                    if (!IsSupported(context.HostingEnvironment)) { return; }

                    configureDelegate(context, configBuilder);
                });
            return this;
        }

        public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            UnderlyingBuilder.ConfigureServices(
                (context, services) =>
                {
                    if (!IsSupported(context.HostingEnvironment)) { return; }

                    configureServices(services);
                });
            return this;
        }

        public IWebHostBuilder ConfigureServices(
            Action<WebHostBuilderContext, IServiceCollection> configureServices)
        {
            UnderlyingBuilder.ConfigureServices(
                (context, services) =>
                {
                    if (!IsSupported(context.HostingEnvironment)) { return; }

                    configureServices(context, services);
                });
            return this;
        }

        public string GetSetting(string key) => UnderlyingBuilder.GetSetting(key);

        IWebHostBuilder IWebHostBuilder.UseSetting(string key, string value) =>
            throw new InvalidOperationException(
                "Cannot call UseSetting in an environment aware block.");
    }
}