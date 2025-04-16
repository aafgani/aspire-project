using Riok.Mapperly.Abstractions;
using Web.API.Dtos.Artist;

namespace Web.API.Dtos.Mapper
{
    [Mapper]
    public partial class ArtistMapper
    {
        public partial App.Infrastructure.Entities.Artist ToEntity(CreateArtistDto dto);
    }
}
