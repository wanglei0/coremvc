using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using WebApp.Deployment.Development;
using WebApp.Deployment.Production;

namespace WebApp.Deployment
{
    public class WebHostConfiguratorFactory
    {
        readonly Dictionary<string, IWebHostConfigurator>
            configurators = new Dictionary<string, IWebHostConfigurator>();

        public WebHostConfiguratorFactory()
            : this(new Dictionary<string, IWebHostConfigurator>
            {
                { EnvironmentName.Development, new DevelopmentWebHostConfigurator() },
                { EnvironmentName.Production, new ProductionWebHostConfigurator() }
            })
        {
        }
        
        public WebHostConfiguratorFactory(IDictionary<string, IWebHostConfigurator> configurators)
        {
            foreach ((string key, IWebHostConfigurator value) in configurators)
            {
                this.configurators.TryAdd(key, value);
            }
        }

        public IWebHostConfigurator Create(string environmentName)
        {
            if (environmentName == null)
            {
                throw new ArgumentNullException(nameof(environmentName));
            }
            
            if (configurators.TryGetValue(environmentName, out IWebHostConfigurator configurator))
            {
                return configurator;
            }
            
            throw new ArgumentException($"Cannot find configurators for environment: {environmentName}");
        }
    }
}