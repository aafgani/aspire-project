using App.Domain.Interface;
using App.Domain.Interface.Repo;
using App.Infrastructure.Cache;
using App.Infrastructure.Data;
using App.Infrastructure.Data.Repo;
using App.Infrastructure.HealthChecks;
using App.Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace App.Infrastructure.Extensions
{
    public static class RegisterServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServicesForWebApp(this IServiceCollection services)
        {
            // Register services for web application
            services.AddSingleton<ICacheService, CacheService>();   
            services.AddSingleton<IUserSessionService, UserSessionService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbRepo(configuration);

            return services;
        }

        private static IServiceCollection AddDbRepo(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Register DbContext with SQL Server
            services.AddDbContext<ChinookDb>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ChinookDb"));
            });

            services.AddScoped<IAlbumRepository, AlbumRepository>();

            return services;
        }

        public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck<ChinookDb>>("ChinookDb-database-health-check", failureStatus : HealthStatus.Unhealthy, tags : new[] { "database" });
          
            return services;
        }
    }
}
