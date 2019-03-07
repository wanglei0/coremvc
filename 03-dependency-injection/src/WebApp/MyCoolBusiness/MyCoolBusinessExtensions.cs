using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.MyCoolBusiness
{
    public static class MyCoolBusinessExtensions
    {
        public static IServiceCollection AddMyCoolBusiness(this IServiceCollection services)
        {
            services.AddTransient<IOutputCreator, OutputCreator>();
            return services;
        }

        public static IApplicationBuilder UseMyCoolBusiness(this IApplicationBuilder app)
        {
            app.Run(
                httpContext =>
                {
                    var outputCreator = httpContext.RequestServices.GetService<IOutputCreator>();

                    PathString requestPath = httpContext.Request.Path;
                    if (requestPath.HasValue && requestPath.Value.EndsWith("cool"))
                    {
                        return httpContext.Response.WriteAsync(outputCreator.CreateMessage());
                    }

                    return httpContext.Response.WriteAsync("Bad Request");
                });
            return app;
        }
    }
}