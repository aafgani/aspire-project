using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Web.API.Dtos.Albums;
using Web.API.Dtos.Mapper;

namespace Web.API.Endpoints
{
    public static class AlbumEndpoints
    {
        public static RouteGroupBuilder MapAlbums(this IEndpointRouteBuilder routes) 
        {
            var group = routes.MapGroup(EndpointGroupNames.AlbumsGroupName);

            group.MapGet("/", async (ChinookDb db, AlbumMapper mapper) =>
            {
                var albums = await db.Albums
                    .Include(a => a.Artist)
                    .ToListAsync();
                var result = albums.Select(mapper.ToDto);
                return Results.Ok(result);
            });

            group.MapGet("/{id:int}", async (ChinookDb db, int id, AlbumMapper mapper) =>
            {
                var album = await db.Albums
                    .Include(a => a.Artist)
                    .FirstOrDefaultAsync(a => a.AlbumId == id);
                var result = mapper.ToDto(album);   
                return album is null ? Results.NotFound() : Results.Ok(result);
            });

            group.MapPost("/", async (ChinookDb db, AlbumDto dto, AlbumMapper mapper) =>
            {
                var album = mapper.ToEntity(dto);
                db.Albums.Add(album);
                await db.SaveChangesAsync();
                return Results.Created($"/albums/{album.AlbumId}", album);
            });

            group.MapPut("/{id:int}", async (ChinookDb db, int id, AlbumDto dto, AlbumMapper mapper) =>
            {
                var album = await db.Albums.FindAsync(id);
                if (album is null) return Results.NotFound();

                album.Title = dto.Title;
                album.ArtistId = dto.ArtistId;
                await db.SaveChangesAsync();

                return Results.Ok(album);
            });

            group.MapDelete("/{id:int}", async (ChinookDb db, int id) =>
            {
                var album = await db.Albums.FindAsync(id);
                if (album is null) return Results.NotFound();

                db.Albums.Remove(album);
                await db.SaveChangesAsync();

                return Results.Ok();
            });

            return group;
        }
    }
}
