using App.Domain.Entities;
using App.Domain.Interface.Repo;

namespace App.Infrastructure.Data.Repo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ChinookDb _db;

        public Repository(ChinookDb db)
        {
            _db = db;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public Task DeleteAsync(Album album)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Album>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
