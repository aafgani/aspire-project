using App.Domain.Entities;
using App.Domain.Interface.Repo;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Repo
{
    public class AlbumRepository : Repository<Album>, IAlbumRepository
    {
        public AlbumRepository(ChinookDb db) : base(db)
        {
        }

        public async Task<Album> GetAlbumWithTracksAsync(int albumId)
        {
            var album = await Set
                .Include(a => a.Tracks)
                .FirstOrDefaultAsync(a => a.AlbumId == albumId);

            return album ?? throw new InvalidOperationException($"Album with ID {albumId} not found.");
        }

        public override async Task<IEnumerable<Album>> GetAllAsync()
        {
            return await Set
                .Include(a => a.Artist)
                .Include(a => a.Tracks)
                .ToListAsync();
        }
    }
}
