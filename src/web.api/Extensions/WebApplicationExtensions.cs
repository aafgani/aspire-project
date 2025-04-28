using App.Domain.CustomException;
using Microsoft.AspNetCore.Diagnostics;
using web.api.Endpoints;
using Web.API.Endpoints;

namespace web.api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapHello();
        app.MapTodos();
        app.MapArtists();
        app .MapAlbums();
        app.MapTracks();
        app.MapCustomers();

        return app;
    }

    public static WebApplication AddCentralizedExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            app.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>();
                context.Response.ContentType = "application/json";

                if (exception is DomainValidationException validationEx)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = "Validation failed",
                        errors = validationEx.Errors
                    });
                }
                else if (exception?.Error is ExternalApiException apiEx)
                {
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    await context.Response.WriteAsJsonAsync(new { message = "External API failed", detail = apiEx.Message });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { message = "Unexpected error occurred" });
                }
            });
        });
       
        return app;
    }
}
