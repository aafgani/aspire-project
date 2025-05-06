using App.Domain.Model.Dtos.Tracks;

namespace App.Domain.Model.Dtos.Albums
{
    public class AlbumDto
    {
        public string Title { get; set; } = default!;

        public int ArtistId { get; set; }
    }

    public class CreateAlbumDto : AlbumDto
    {

    }

    public class UpdateAlbumDto : AlbumDto
    {

    }

    public class AlbumResponseDto
    {
        public int AlbumId { get; set; }
        public string Title { get; set; } = default!;
        public string ArtistName { get; set; } = default!;
        public ICollection<TrackDto> Tracks { get; set; } = default!;
    }
}
