using web.api.Endpoints;

namespace web.api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapTodos();
        app.MapHello();

        return app;
    }
}
