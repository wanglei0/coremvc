using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp
{
    public class Startup
    {
        readonly ILogger<Startup> logger;

        public Startup(ILogger<Startup> logger) { this.logger = logger; }
        
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {   
            app.Run(c =>
            {
                logger.LogWarning("Oh god I am {logger}", logger.ToString());
                return c.Response.WriteAsync("Hello World");
            });
        }
    }
}
