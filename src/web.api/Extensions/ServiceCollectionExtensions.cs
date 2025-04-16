using App.Domain.Model.Configuration;
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
        services
            .Configure<StorageConfiguration>(configuration.GetSection("StorageConfiguration"))
            .AddOptionsWithValidateOnStart<StorageConfiguration>()
            .ValidateDataAnnotations();
        return services;
    }
}
