using App.Domain.Entities;

namespace App.Domain.Interface.Repo
{
    public interface IAlbumRepository : IRepository<Album>
    {
        Task<Album> GetAlbumWithTracksAsync(int albumId);
    }
}
