using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Deployment
{
    public interface IEnvironmentSpecificStartup
    {
        void Configure(IApplicationBuilder app);
        void ConfigureServices(IServiceCollection services);
    }
}