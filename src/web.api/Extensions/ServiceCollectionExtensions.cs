using web.api.Configurations;
using web.api.Services;

namespace web.api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ITodoTableService, TodoTableService>();
        return services;
    }
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<StorageConfiguration>(configuration.GetSection("StorageConfiguration"));
        return services;
    }

    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, ConfigurationManager configuration)
    {
        // const string DatabaseName = "DbName";
        // var xwingConnectionString = configuration.GetConnectionString(DatabaseName);
        return services;
    }
}
