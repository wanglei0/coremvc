using System;
using System.Collections.Generic;

namespace WebApp.Deployment
{
    public abstract class WebHostConfiguratorFactory
    {
        readonly Dictionary<string, IWebHostConfigurator>
            configurators = new Dictionary<string, IWebHostConfigurator>();

        protected WebHostConfiguratorFactory(IDictionary<string, IWebHostConfigurator> configurators)
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