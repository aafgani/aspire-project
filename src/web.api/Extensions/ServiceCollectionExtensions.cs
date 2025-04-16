using App.Domain.Model.Configuration;
using web.api.Services;
using Web.API.Dtos.Mapper;

namespace web.api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ITodoTableService, TodoTableService>();
        services.AddMapper();
        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddScoped<ArtistMapper>();

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
