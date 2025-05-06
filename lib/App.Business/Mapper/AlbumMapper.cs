using App.Domain.Entities;
using App.Domain.Interface.Mapper;
using App.Domain.Model.Dtos.Albums;
using App.Domain.Model.Dtos.Tracks;
using Riok.Mapperly.Abstractions;

namespace App.Business.Mapper
{
    [Mapper]
    public partial class AlbumMapper : IAlbumMapper
    {

        public partial Album ToEntity(AlbumDto dto);

        [MapProperty(nameof(Album.Tracks), nameof(AlbumResponseDto.Tracks))]
        public partial AlbumResponseDto ToDto(Album album);

        // Manually map tracks to TrackDto
        public static TrackDto MapTrack(Track track)
        {
            return new TrackDto
            {
                Name = track.Name
            };
        }
    }
}
