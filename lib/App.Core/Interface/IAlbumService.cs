using App.Domain.Model.Dtos.Albums;

namespace App.Domain.Interface
{
    public interface IAlbumService
    {
        Task<AlbumResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<AlbumResponseDto>> GetAllAsync();
        Task<AlbumResponseDto> CreateAsync(CreateAlbumDto dto);
        Task<bool> UpdateAsync(int id, UpdateAlbumDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
