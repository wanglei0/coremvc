using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Infrastructure.Test.Deployment.Helpers
{
    class NullStartup
    {
        public void Configure(IApplicationBuilder app) { }
        public void ConfigureServices(IServiceCollection services) { }
    }
}