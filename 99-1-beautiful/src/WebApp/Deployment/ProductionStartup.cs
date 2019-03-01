using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp.Deployment
{
    class ProductionStartup : IStartupForEnvironment
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(c =>
            {
                var logger = c.RequestServices.GetService<ILogger<ProductionStartup>>();
                logger.LogWarning("Hello Warning!");
                return c.Response.WriteAsync("Hello Production");
            });
        }

        public void ConfigureServices(IServiceCollection services) { }
    }
}