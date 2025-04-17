using App.Domain.Interface;
using App.Domain.Model.Dtos.Tracks;

namespace Web.API.Endpoints
{
    public static class TracksEndpoints
    {
        public static RouteGroupBuilder MapTracks(this IEndpointRouteBuilder routes) 
        {
            var group = routes.MapGroup(EndpointGroupNames.TracksGroupName);

            group.MapGet("/top-tracks", async (int count, ITrackService service) =>
            {
                var result = await service.GetTopTracks(count);
                return result is null ? Results.NotFound() : Results.Ok(result);
            })
                .WithName("GetTopTracks")
                .WithSummary("Get the top most purchased tracks")
                .WithDescription("Returns a list of the top purchased tracks based on invoice lines, including track name, purchase count, and artist.")
                .WithOpenApi();

            return group;
        }
    }
}
