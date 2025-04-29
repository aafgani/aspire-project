using App.Business.Mapper;
using App.Domain.Interface;
using App.Domain.Interface.Mapper;
using App.Domain.Interface.Repo;
using App.Domain.Model.Dtos.Albums;

namespace App.Business.Features.Albums
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IAlbumMapper _mapper;

        public AlbumService(IAlbumRepository albumRepository, IAlbumMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }

        public async Task<AlbumResponseDto> CreateAsync(CreateAlbumDto dto)
        {
            var album = _mapper.ToEntity(dto);
            await _albumRepository.CreateAsync(album);
            return _mapper.ToDto(album);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var album = await _albumRepository.GetByIdAsync(id);
            if (album is null) return false;

            await _albumRepository.DeleteAsync(album);
            return true;
        }

        public async Task<IEnumerable<AlbumResponseDto>> GetAllAsync()
        {
            var albums = await _albumRepository.GetAllAsync();
            return albums.Select(_mapper.ToDto);
        }

        public async Task<AlbumResponseDto?> GetByIdAsync(int id)
        {
            var album = await _albumRepository.GetByIdAsync(id);
            return album is null ? null : _mapper.ToDto(album);
        }

        public async Task<bool> UpdateAsync(int id, UpdateAlbumDto dto)
        {
            var album = await _albumRepository.GetByIdAsync(id);
            if (album is null) return false;

            album.Title = dto.Title;
            album.ArtistId = dto.ArtistId;
            await _albumRepository.UpdateAsync(album);
            return true;
        }
    }
}
