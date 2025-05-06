using App.Domain.Entities;
using Shouldly;
using System.Net.Http.Json;
using Test.IntegrationTest.Api.Fixture;

namespace Test.IntegrationTest.Api.FeaturesTests.ArtistsTests
{
    public class GetArtistTests : BaseIntegrationTest
    {
        public GetArtistTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAllArtists_ReturnsOk()
        {
            // Arrange
            // seed test data
            ChinookDbContext.Artists.AddRange(
                new Artist { Name = "The Rolling Stones" },
                new Artist { Name = "Led Zeppelin" },
                new Artist { Name = "Pink Floyd" }
            );
            await ChinookDbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetAsync("/artists");

            // Assert
            response.EnsureSuccessStatusCode();
            var artists = await response.Content.ReadFromJsonAsync<List<Artist>>();
            artists.ShouldNotBeNull();
            artists.Count.ShouldBe(3);
        }
    }
}
