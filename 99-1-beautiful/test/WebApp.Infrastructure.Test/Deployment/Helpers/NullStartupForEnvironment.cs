using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Deployment;

namespace WebApp.Infrastructure.Test.Deployment.Helpers
{
    class NullStartupForEnvironment : IStartupForEnvironment
    {
        public void Configure(IApplicationBuilder app) { }
        public void ConfigureServices(IServiceCollection services) { }
    }
}