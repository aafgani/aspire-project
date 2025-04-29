using App.Business.Mapper;
using App.Domain.Model.Dtos.Artists;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.API.Endpoints
{
    public static class ArtistEndpoints
    {
        public static RouteGroupBuilder MapArtists(this IEndpointRouteBuilder routes) 
        {
            var group = routes
                .MapGroup(EndpointGroupNames.ArtistsGroupName)
                .WithTags(EndpointGroupNames.ArtistsTagName);

            group.MapGet("/", async (ChinookDb db) =>
            {
                var artists = await db.Artists.ToListAsync();
                return Results.Ok(artists);
            });

            group.MapGet("/{id:int}", async (int id, ChinookDb db) =>
            {
                var artist = await db.Artists.FindAsync(id);
                return artist is not null ? Results.Ok(artist) : Results.NotFound();
            });

            group.MapPost("/", async (CreateArtistDto dto, ChinookDb db, ArtistMapper mapper) =>
            {
                var artist = mapper.ToEntity(dto);

                db.Artists.Add(artist);
                await db.SaveChangesAsync();

                return Results.Created($"/artists/{artist.ArtistId}", artist);
            });

            group.MapDelete("/{id:int}", async (int id, ChinookDb db) =>
            {
                var artist = await db.Artists.FindAsync(id);
                if (artist is null) return Results.NotFound();

                db.Artists.Remove(artist);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return group;
        }
    }
}
