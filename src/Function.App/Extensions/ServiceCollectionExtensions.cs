using App.Domain.Core;
using App.Domain.Core.Behaviour;
using App.Domain.Features.MoneyManager.UploadSpreadsheet;
using App.Domain.Interface;
using App.Function.Configurations;
using FluentValidation;
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

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            
            // Register validators
            services.AddScoped<IValidator<UploadSpreadsheetCommand>, UploadSpreadsheetCommandValidator>();

            // Handlers
            services.AddScoped<ICommandHandler<UploadSpreadsheetCommand>, UploadSpreadsheetCommandHandler>();
            
            // Pipeline decorators (order matters)
            services.Decorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
            services.Decorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));

            return services;
        }
    }
}
