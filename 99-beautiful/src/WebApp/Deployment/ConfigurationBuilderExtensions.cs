using Microsoft.Extensions.Configuration;

namespace WebApp.Deployment
{
    static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEnvironmentSpecificJsonConfiguration(
            this IConfigurationBuilder configurationBuilder, string environmentString)
        {
            return configurationBuilder.AddJsonFile($"config.{environmentString}.json");
        }
    }
}