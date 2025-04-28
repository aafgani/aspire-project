using App.Business.Mapper;
using App.Domain.Interface.Mapper;
using App.Domain.Interface.Repo;
using Moq;

namespace Test.UnitTest.Business.FeatureTests.AlbumServiceTests
{
    public class BaseAlbumServiceTests
    {
        protected (Mock<IAlbumRepository> IAlbumRepository,Mock< IAlbumMapper> mapper) GetDependencies()
        {
            var mockAlbumRepository = new Mock<IAlbumRepository>();
            var mapper = new Mock<IAlbumMapper>();
            return (mockAlbumRepository, mapper);
        }
    }
}
