using App.Infrastructure.Entities;
using Riok.Mapperly.Abstractions;
using Web.API.Dtos.Albums;
namespace Web.API.Dtos.Mapper
{
    [Mapper]
    public partial class AlbumMapper
    {
        public partial Album ToEntity(AlbumDto dto);

        public partial AlbumResponseDto ToDto(Album album);
    }
}
