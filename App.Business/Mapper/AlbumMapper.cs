using App.Domain.Entities;
using App.Domain.Model.Dtos.Albums;
using Riok.Mapperly.Abstractions;

namespace App.Business.Mapper
{
    [Mapper]
    public partial class AlbumMapper
    {

        public partial Album ToEntity(AlbumDto dto);

        public partial AlbumResponseDto ToDto(Album album);
    }
}
