using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Deployment
{
    public interface IStartupForEnvironment
    {
        void Configure(IApplicationBuilder app);
        void ConfigureServices(IServiceCollection services);
    }
}