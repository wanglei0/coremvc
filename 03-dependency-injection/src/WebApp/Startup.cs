using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApp.MyCoolBusiness;

namespace WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMyCoolBusiness();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMyCoolBusiness();
        }
    }
}
