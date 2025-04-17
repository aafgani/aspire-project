using App.Business.Features.MoneyManager.UploadSpreadsheet;
using App.Business.Mapper;
using App.Domain.Core.Behaviour;
using App.Domain.Core;
using App.Domain.Interface;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Business.Validator;
using App.Business.Features.Albums;
using App.Business.Features.Tracks;

namespace App.Business.Extensions
{
    public static class RegisterServiceExtensions
    {
        public static IServiceCollection AddBusinesServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMapper()
                .AddClassServices()
                .AddHandlers();

            return services;
        }

        private static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddScoped<ArtistMapper>();
            services.AddScoped<AlbumMapper>();
            services.AddScoped<TrackMapper>();

            return services;
        }

        private static IServiceCollection AddClassServices(this IServiceCollection services)
        {
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<ITrackService, TrackService>();

            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
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
