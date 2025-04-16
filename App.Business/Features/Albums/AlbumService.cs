using App.Business.Mapper;
using App.Domain.Interface;
using App.Domain.Model;
using App.Domain.Model.Dtos.Albums;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Business.Features.Albums
{
    public class AlbumService : IAlbumService
    {
        private readonly ChinookDb _db ;
        private readonly AlbumMapper _mapper;

        public AlbumService(ChinookDb db, AlbumMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<AlbumResponseDto> CreateAsync(CreateAlbumDto dto)
        {
            var album = _mapper.ToEntity(dto);
            _db.Albums.Add(album);
            await _db.SaveChangesAsync();
            return _mapper.ToDto(album);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var album = await _db.Albums.FindAsync(id);
            if (album is null) return false;

            _db.Albums.Remove(album);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AlbumResponseDto>> GetAllAsync()
        {
            var albums = await _db.Albums
                    .Include(a => a.Artist)
                    .ToListAsync();
            var result = albums.Select(_mapper.ToDto);

            return result;
        }

        public async Task<AlbumResponseDto?> GetByIdAsync(int id)
        {
            var album = await _db.Albums
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(a => a.AlbumId == id);

            return album is null ? null : _mapper.ToDto(album);
        }

        public async Task<bool> UpdateAsync(int id, UpdateAlbumDto dto)
        {
            var album = await _db.Albums.FindAsync(id);
            if (album is null) return false;

            album.Title = dto.Title;
            album.ArtistId = dto.ArtistId;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
