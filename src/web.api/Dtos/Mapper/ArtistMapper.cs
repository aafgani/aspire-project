using App.Infrastructure.Entities;
using Riok.Mapperly.Abstractions;
using Web.API.Dtos.Artists;

namespace Web.API.Dtos.Mapper
{
    [Mapper]
    public partial class ArtistMapper
    {
        public partial Artist ToEntity(CreateArtistDto dto);
    }
}
