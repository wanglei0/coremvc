using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Deployment;
using Xunit;

namespace WebApp.Test
{
    public class WebHostConfiguratorFacts
    {
        class ConfiguratorFactory : WebHostConfiguratorFactory
        {
            public ConfiguratorFactory(IDictionary<string, IWebHostConfigurator> configurators) : base(configurators)
            {
            }
        }

        class EnvironmentalConfigurator : IWebHostConfigurator
        {
            public string EnvironmentName { get; }
            public IList<string> Messages { get; } = new List<string>();

            public EnvironmentalConfigurator(string environmentName)
            {
                EnvironmentName = environmentName;
            }
            
            public void ConfigureLogging(WebHostBuilderContext context, IServiceCollection services)
            {
                Record(nameof(ConfigureLogging));
            }

            public void ConfigureConfiguration(WebHostBuilderContext context, IConfigurationBuilder configurationBuilder)
            {
                Record(nameof(ConfigureConfiguration));
            }

            void Record(string message)
            {
                Messages.Add($"{EnvironmentName}:{message}");
            }
        }
        
        [Fact]
        public void should_dispatching_configure_process_to_different_environment()
        {
            var factory = new ConfiguratorFactory(new Dictionary<string, IWebHostConfigurator>
            {
                { "Development", new EnvironmentalConfigurator("Development") },
                { "Production", new EnvironmentalConfigurator("Production") }
            });

            new WebHostBuilder()
                .UseEnvironment("Development")
                .ConfigureServices((ctx, s) =>
                    factory.Create(ctx.HostingEnvironment.EnvironmentName).ConfigureLogging(ctx, s))
                .Configure(app => { })
                .Build();

            var developmentConfigurator = (EnvironmentalConfigurator) factory.Create("Development");
            var productionConfigurator = (EnvironmentalConfigurator) factory.Create("Production");
            
            Assert.Empty(productionConfigurator.Messages);
            Assert.Equal("Development:ConfigureLogging", developmentConfigurator.Messages.Single());
        }
    }
}