using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebApp.ExceptionHandler
{
    static class ApiExceptionHandlerMiddleware
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder appBuilder)
        {
            // This can only cover error response caused by exception and its error header
            // has not been sent (it will not cover streaming scenario).
            //
            // It is highly recommend to use simple static content to avoid exceptions when
            // generating error content.
            return appBuilder.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = HandleException
            });
        }
        
        static Task HandleException(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{{\"message\":\"error occured\"}}");
        }
    }
}