using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebModule.HealthCheck
{
    static class HealthCheckResponseWriter
    {
        public static Task WriteAsync(HttpContext context, HealthReport report)
        {
            // You can change the format of the health check report here. This is only an example.
            // You can change it so that the health check result can be easily adopted by the
            // monitoring service.
            //
            // Currently, there is no standard for health check response. A draft standard is
            // purposed but is presently in a very early stage
            // (https://datatracker.ietf.org/doc/draft-inadarei-api-health-check).
            
            context.Response.ContentType = "text/plain";
            StringBuilder content = new StringBuilder()
                .Append(report.Status).Append('\n')
                .Append(
                    string.Join(
                        '\n',
                        report.Entries.Select(e => $"{e.Key}:{e.Value.Status}")));
            return context.Response.WriteAsync(content.ToString());
        }
    }
}