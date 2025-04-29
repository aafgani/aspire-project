using App.Domain.Entities;

namespace App.Domain.Interface.Repo
{
    public interface IRepository<T>
    {
        Task<IEnumerable<Album>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task DeleteAsync(Album album);
    }
}