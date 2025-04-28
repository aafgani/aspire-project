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
            // maybe seed some test data if needed

            // Act
            var response = await Client.GetAsync("/artists");

            // Assert
            response.EnsureSuccessStatusCode();
            var artists = await response.Content.ReadFromJsonAsync<List<Artist>>();
            artists.ShouldNotBeNull();
            artists.Count.ShouldBeGreaterThan(0); 
        }
    }
}
