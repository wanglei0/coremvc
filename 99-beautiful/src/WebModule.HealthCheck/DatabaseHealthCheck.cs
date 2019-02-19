using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace WebModule.HealthCheck
{
    [SuppressMessage(
        "ReSharper",
        "ClassNeverInstantiated.Global",
        Justification = "It will be resolved by DI")]
    class DatabaseHealthCheck : IHealthCheck
    {
        readonly IOptionsSnapshot<HealthCheckConfig> config;

        public DatabaseHealthCheck(IOptionsSnapshot<HealthCheckConfig> config)
        {
            this.config = config;
        }
        
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken)
        {
            string connectionString = config.Value?.ConnectionString;
            
            if (string.IsNullOrEmpty(connectionString))
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy("Database connection string empty."));
            }

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT 'Go'";
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    object value = command.ExecuteScalar();
                    if (!(value is string stringValue))
                    {
                        return Task.FromResult(
                            HealthCheckResult.Unhealthy(
                                "Invalid Execution Result: No return value"));
                    }

                    if (stringValue != "Go")
                    {
                        return Task.FromResult(
                            HealthCheckResult.Unhealthy(
                                $"Invalid Execution Result: \"{stringValue}\""));
                    }
                }

                return Task.FromResult(HealthCheckResult.Healthy("All Test Passed"));
            }
            catch (Exception error)
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy("Unhandled error occured", error));
            }
        }
    }
}