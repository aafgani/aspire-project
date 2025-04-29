using App.Domain.Entities;
using Test.IntegrationTest.Infrastructure.Fixture;

namespace Test.IntegrationTest.Infrastructure.Tests.AlbumRepositoryTests
{
    public class CreateAsyncTests
    {
        private readonly TestDbFixture _fixture;

        public CreateAsyncTests(TestDbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateAlbum_ShouldPersistToDatabase()
        {
            // Arrange
            var db = _fixture.MsSqlContainer;

            var newAlbum = new Album
            {
                Title = "Integration Album",
                ArtistId = 1
            };

            db.Artists.Add(new Artist { ArtistId = 1, Name = "Test Artist" });
            await db.SaveChangesAsync();

            // Act
            db.Albums.Add(newAlbum);
            await db.SaveChangesAsync();

            // Assert
            var albumInDb = await db.Albums.Include(a => a.Artist).FirstOrDefaultAsync(a => a.Title == "Integration Album");
            albumInDb.Should().NotBeNull();
            albumInDb.Artist.Name.Should().Be("Test Artist");
        }
    }
}
