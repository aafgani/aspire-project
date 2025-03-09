using web.api.Models;

namespace web.api.Services;

public interface ITodoTableService
{
    Task<Todo> GetByIdAsync(string rowKey);
    Task Upsert(Todo todo);
    Task<List<Todo>> GetByEmailAsync(string email);
    Task<List<Todo>> GetAll();
    Task CompleteItem(string rowKey);
    Task DeleteItem(string rowKey);

}
