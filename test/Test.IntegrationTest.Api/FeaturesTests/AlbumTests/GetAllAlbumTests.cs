using App.Domain.Entities;
using App.Domain.Model.Dtos.Albums;
using App.Infrastructure.Data;
using Shouldly;
using System.Net.Http.Json;
using Test.IntegrationTest.Api.Fixture;

namespace Test.IntegrationTest.Api.FeaturesTests.AlbumTests
{
    public class GetAllAlbumTests : BaseIntegrationTest
    {
        public GetAllAlbumTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GivenGetAlbumRequest_GetAsync_ReturnsOk()
        {
            // Arrange
            await SeedArtistWithAlbumAndTracksAsync(ChinookDbContext);

            // Act
            var response = await Client.GetAsync($"/albums/");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultAlbum = await response.Content.ReadFromJsonAsync<IEnumerable<AlbumResponseDto>>();
            resultAlbum.ShouldNotBeNull();
            resultAlbum.Count().ShouldBe(1);
            resultAlbum.First().ArtistName.ShouldBe("Test Artist");
            resultAlbum.First().Tracks.Count.ShouldBe(2);
        }

        private async Task SeedArtistWithAlbumAndTracksAsync(ChinookDb context)
        {
            var mediaType = new MediaType
            {
                Name = "Test MediaType"
            };

            var artist = new Artist { Name = "Test Artist" };

            var album = new Album
            {
                Title = "Test Album",
                Tracks = new List<Track>
                {
                    new Track { Name = "Test Track 1", MediaType = mediaType },
                    new Track { Name = "Test Track 2", MediaType = mediaType }
                }
            };

            // Link the album to the artist
            artist.Albums = new List<Album> { album };

            context.Artists.Add(artist);
            await context.SaveChangesAsync();
        }
    }
}
