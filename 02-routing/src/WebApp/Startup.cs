using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // This method gets called by the runtime. Use this method to add services to
            // the container.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // This method gets called by the runtime. Use this method to configure the HTTP
            // request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc(ConfigureRoutes);
        }

        void ConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                "map-to-get",
                "api/centralized-method",
                new {controller = "CentralizedMethodRouting", action = "Get"},
                new {httpMethod = new HttpMethodRouteConstraint("GET")});
            
            routes.MapRoute(
                "map-to-eat",
                "api/centralized-method",
                new {controller = "CentralizedMethodRouting", action = "Eat"},
                new {httpMethod = new HttpMethodRouteConstraint("EAT")});
            
            routes.MapRoute(
                "map-to-multiple-methods",
                "api/centralized-method/multiple",
                new {controller = "CentralizedMethodRouting", action = "MapToMultipleMethods"},
                new {httpMethod = new HttpMethodRouteConstraint("GET", "POST", "DELETE")});
        }
    }
}
