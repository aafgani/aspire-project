using App.Domain.Entities;
using App.Domain.Model.Dtos.Albums;

namespace App.Domain.Interface.Mapper
{
    public interface IAlbumMapper
    {
        Album ToEntity(AlbumDto dto);
        AlbumResponseDto ToDto(Album album);
    }
}
