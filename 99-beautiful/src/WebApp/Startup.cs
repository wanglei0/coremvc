using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // In order to create mvc related instances, we should add these definitions to
            // DI services. This is kind of a shared infrastructure.
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // In order to let mvc come into play. We should add mvc middleware to the pipeline.
            app.UseMvc();
        }
    }
}