using App.Domain.Entities;
using App.Domain.Model.Dtos.Artists;
using Riok.Mapperly.Abstractions;

namespace App.Business.Mapper
{
    [Mapper]
    public partial class ArtistMapper
    {
        public partial Artist ToEntity(CreateArtistDto dto);
    }
}
