using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp.Deployment
{
    abstract class EnvironmentAwareStartup
    {
        public IHostingEnvironment HostingEnvironment { get; set; }
        public IConfiguration Configuration { get; set; }
        public ILoggerFactory Factory { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            if (!IsSupported()) { return; }
            ConfigureAppServices(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (!IsSupported()) { return; }
            ConfigureAppPipeline(app);
        }
        
        protected abstract void ConfigureAppServices(IServiceCollection services);
        protected abstract void ConfigureAppPipeline(IApplicationBuilder app);
        protected abstract bool IsSupported();
    }
}