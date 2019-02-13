using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Plugins;

namespace WebModule.HealthCheck
{
    public class Startup : IModuleStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DatabaseHealthCheck>();
            
            // You can add more health check services. One per IHealthCheck implementation.
            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("database");
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.WriteAsync
            });
        }
    }
}