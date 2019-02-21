using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace WebApp.TestBase
{
    class WebAppServiceConfigurator : ITestServiceConfigurator
    {   
        public void ConfigureService( 
            IServiceCollection services,
            IDictionary<string, object> dependencies)
        {
            TestLoggingConfiguration GetLoggingConfiguration()
            {
                return (TestLoggingConfiguration) dependencies[TestServiceConfiguratorKeys
                    .LoggingConfiguration];
            }

            ITestOutputHelper GetTestOutputHelper()
            {
                return (ITestOutputHelper) dependencies[TestServiceConfiguratorKeys.TestOutputHelper];
            }

            HttpMessageHandler GetExternalServices()
            {
                return (HttpMessageHandler) dependencies[TestServiceConfiguratorKeys.ExternalService];
            }

            ResetLogging(services, GetLoggingConfiguration(), GetTestOutputHelper());
            UseHttpClientToExternalSystem(services, GetExternalServices());
        }

        static void ResetLogging(
            IServiceCollection services,
            TestLoggingConfiguration loggingConfiguration,
            ITestOutputHelper output)
        {
            services.RemoveAll<ILoggerFactory>();
            services.RemoveAll(typeof(ILogger<>));
            
            services.AddLogging(
                lb =>
                {
                    lb.ClearProviders();
                    if (loggingConfiguration.EnableLogging)
                    {
                        lb.AddProvider(
                                new XUnitLoggerProvider(output, loggingConfiguration.SupportScope))
                            .SetMinimumLevel(loggingConfiguration.MinimumLevel);
                    }
                });
        }

        static void UseHttpClientToExternalSystem(
            IServiceCollection services,
            HttpMessageHandler externalServices)
        {
            services.AddSingleton(_ => new HttpClient(externalServices));
        }
    }
}