using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Plugins;

namespace WebModule.HealthCheck
{
    public class Startup : IModuleStartup
    {
        readonly IConfiguration configuration;
        
        public Startup(IConfiguration configuration) { this.configuration = configuration; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DatabaseHealthCheck>();
            services.Configure<HealthCheckConfig>(configuration.GetSection("Database"));
            
            // You can add more health check services. One per IHealthCheck implementation.
            services.AddHealthChecks().AddCheck<DatabaseHealthCheck>("database");
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