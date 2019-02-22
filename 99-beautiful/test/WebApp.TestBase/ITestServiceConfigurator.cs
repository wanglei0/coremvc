using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.TestBase
{
    public interface ITestServiceConfigurator
    {
        void ConfigureService(
            IServiceCollection services,
            IDictionary<string, object> dependencies);

        void ConfigureConfiguration(
            WebHostBuilderContext context,
            IConfigurationBuilder configurationBuilder);
    }
}