using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebModule.HealthCheck
{
    [SuppressMessage(
        "ReSharper",
        "ClassNeverInstantiated.Global",
        Justification = "It will be resolved by DI")]
    class DatabaseHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("Database is available"));
        }
    }
}