using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebApp.ExceptionHandling
{
    static class ApiExceptionResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiExceptionResponse(this IApplicationBuilder appBuilder)
        {
            // The exception handling can be triggered by:
            // 
            // (1) The explicit known exception in the business logic.
            // (2) The unhandled exception.
            //
            // The explicit known exception should be handled in the business logic. It should catch
            // the exception, appropriately log the information and then create an error response
            // directly without propagate it.
            //
            // We should consider streaming API to the explicit known exception handling area since
            // the response header will be sent immediately thus there is no chance to modify the
            // response status.
            // 
            // So when we say common exception handling logic, we mean the unhandled exception
            // handling.
            //
            // And the exception handling contains the following parts:
            //
            // (1) Logging and notification.
            // (2) Create error response according to different environment.
            //
            // Unlike previous framework. ASP.NET Core contains sophisticated logging abstractions.
            // So we can only care about how to create error response according to different
            // environments. Or if the exception contains additional information on notification
            // settings, we can send notification in the exception handler.
            return appBuilder.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = HandleException
            });
        }
        
        static Task HandleException(HttpContext context)
        {
            // It is highly recommend to use simple static content to avoid exceptions when
            // generating error content.
            //
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{{\"message\":\"error occured\"}}");
        }
    }
}