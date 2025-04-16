namespace Web.API.Dtos.Albums
{
    public class AlbumDto
    {
        public string Title { get; set; } = default!;

        public int ArtistId { get; set; }
    }

    public class AlbumResponseDto
    {
        public int AlbumId { get; set; }
        public string Title { get; set; } = default!;
        public string ArtistName { get; set; } = default!;
    }
}
