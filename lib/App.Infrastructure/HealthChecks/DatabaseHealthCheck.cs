using App.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace App.Infrastructure.HealthChecks
{
    internal class DatabaseHealthCheck : IHealthCheck
    {
        private readonly IServiceProvider _serviceProvider;
        public DatabaseHealthCheck(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ChinookDb>();
                
                try
                {
                    await dbContext.Database.CanConnectAsync(cancellationToken);
                    return HealthCheckResult.Healthy("Database connection is healthy");

                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy("Database connection failed", ex);
                }
               
            }
            
        }
    }
}