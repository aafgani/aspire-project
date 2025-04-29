using App.Domain.Interface;
using App.Domain.Model.Dtos.Albums;

namespace Web.API.Endpoints
{
    public static class AlbumEndpoints
    {
        public static RouteGroupBuilder MapAlbums(this IEndpointRouteBuilder routes) 
        {
            var group = routes
                .MapGroup(EndpointGroupNames.AlbumsGroupName)
                .WithTags(EndpointGroupNames.AlbumsTagName);

            group.MapGet("/", async (IAlbumService service) =>
            {
                var result = await service.GetAllAsync();
                return Results.Ok(result);
            });

            group.MapGet("/{id:int}", async (IAlbumService service, int id) =>
            {
                var album = await service.GetByIdAsync(id);
                return album is null ? Results.NotFound() : Results.Ok(album);
            });

            group.MapPost("/", async (IAlbumService service, CreateAlbumDto dto) =>
            {
                var album = await service.CreateAsync(dto);
                return Results.Created($"/albums/{album.AlbumId}", album);
            });

            group.MapPut("/{id:int}", async (IAlbumService service, int id, UpdateAlbumDto dto) =>
            {
                var update = await service.UpdateAsync(id, dto);
                if(!update) return Results.NotFound();
            
                return Results.Accepted();
            });

            group.MapDelete("/{id:int}", async (IAlbumService service, int id) =>
            {
                var result = await service.DeleteAsync(id);
                if (!result) return Results.NotFound();

                return Results.Ok();
            });

            return group;
        }
    }
}
