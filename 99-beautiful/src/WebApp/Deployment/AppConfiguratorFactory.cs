using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using WebApp.Deployment.Development;
using WebApp.Deployment.Production;

namespace WebApp.Deployment
{
    public class AppConfiguratorFactory : WebHostConfiguratorFactory
    {
        public AppConfiguratorFactory()
            : this(new Dictionary<string, IWebHostConfigurator>
            {
                { EnvironmentName.Development, new DevelopmentWebHostConfigurator() },
                { EnvironmentName.Production, new ProductionWebHostConfigurator() }
            })
        {
        }
        
        public AppConfiguratorFactory(IDictionary<string, IWebHostConfigurator> configurators) 
            : base(configurators)
        {
        }
    }
}