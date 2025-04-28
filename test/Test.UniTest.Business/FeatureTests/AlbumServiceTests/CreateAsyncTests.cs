using App.Business.Features.Albums;
using App.Domain.Entities;
using App.Domain.Model.Dtos.Albums;
using Moq;
using Shouldly;
using Test.UnitTest.Business.FeatureTests.AlbumServiceTests;

namespace Test.UniTest.Business.FeatureTests.AlbumServiceTests
{
    public class CreateAsyncTests : BaseAlbumServiceTests
    {
        [Fact]
        public async Task GivenValidAlbumDto_CreateAsync_ShouldCreateAlbum()
        {
            // Arrange.
            var createDto = new CreateAlbumDto
            {
                ArtistId = 1,
                Title = "Test Album",
            };
            var newAlbum = new Album { 
                Title = "Test Album", 
                ArtistId = 1, 
            };
            var responseDto = new AlbumResponseDto { Title = "Test Album", ArtistName = "Artist Name" };

            var (mockAlbumRepo, mockMapper) = GetDependencies();
            mockAlbumRepo.Setup(m => m.CreateAsync(It.IsAny<Album>()))
                .Returns(Task.FromResult(newAlbum));
            mockMapper.Setup(m => m.ToEntity(It.IsAny<CreateAlbumDto>()))
                .Returns(newAlbum);
            mockMapper.Setup(m => m.ToDto(It.IsAny<Album>()))
                      .Returns(responseDto);

            var albumService = new AlbumService(mockAlbumRepo.Object, mockMapper.Object);

            // Act.
            var result = await albumService.CreateAsync(createDto);

            // Assert.
            mockAlbumRepo.Verify(m => m.CreateAsync(It.Is<Album>(a =>
                a.Title == "Test Album" &&
                a.ArtistId == 1)));
            result.ShouldNotBeNull();
            result.Title.ShouldBe("Test Album");
            result.ArtistName.ShouldBe("Artist Name");
        }
    }
}
