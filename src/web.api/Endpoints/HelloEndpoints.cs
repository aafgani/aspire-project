namespace web.api.Endpoints;

public static class HelloEndpoints
{
    public static RouteGroupBuilder MapHello(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/hello");

        group.MapGet("/", (IConfiguration Configuration) =>
        {
         var env = Environment.OSVersion?.ToString();
            var config = Configuration["TestEnvVar"]?.ToString();
            return Results.Ok($"Hi, it's me Yayak - {config} - env : {env}");
        });

        return group;
    }
}
