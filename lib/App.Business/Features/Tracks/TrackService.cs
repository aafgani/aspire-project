using App.Business.Mapper;
using App.Domain.Interface;
using App.Domain.Model.Dtos.Tracks;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Business.Features.Tracks
{
    public class TrackService : ITrackService
    {
        private readonly ChinookDb _db;
        private readonly TrackMapper _mapper;

        public TrackService(ChinookDb db, TrackMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TopTrackDto>> GetTopTracks(int top)
        {
            var result = await _db.InvoiceLines
                .GroupBy(il => il.TrackId)
                .Select(g => new {
                    TrackId = g.Key,
                    PurchaseCount = g.Count()
                })
                .OrderByDescending(x => x.PurchaseCount)
                .Take(10)
                .Join(_db.Tracks,
                      g => g.TrackId,
                      t => t.TrackId,
                      (g, t) => new  TopTrackDto{
                          Name = t.Name,
                          PurchaseCount = g.PurchaseCount,
                          Artist = t.Album.Artist.Name
                      })
                .ToListAsync();

            return result;
        }
    }
}
