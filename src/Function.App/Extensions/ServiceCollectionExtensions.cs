using App.Function.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace App.Function.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services)
        {
            services
                .AddOptions<ConnectionStringOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration
                        .GetSection(ConnectionStringOptions.ConfigurationSectionKey)
                        .Bind(options);
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<InitializationService>();
            return services;
        }
    }
}
