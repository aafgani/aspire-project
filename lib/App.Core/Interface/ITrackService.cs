using App.Domain.Model.Dtos.Tracks;

namespace App.Domain.Interface
{
    public interface ITrackService
    {
        Task<IEnumerable<TopTrackDto>> GetTopTracks(int top);
    }
}
