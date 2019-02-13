using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

namespace WebModule.HealthCheck
{
    [SuppressMessage(
        "ReSharper",
        "ClassNeverInstantiated.Global",
        Justification = "It will be resolved by DI")]
    class DatabaseHealthCheck : IHealthCheck
    {
        // Directly use the logger after the application initialization.
        readonly ILogger logger = Log.Logger;
        
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken)
        {
            logger.Information("The database health check is running.");
            return Task.FromResult(
                HealthCheckResult.Healthy("Database is available"));
        }
    }
}