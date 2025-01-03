using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using web.api.Configurations;
using web.api.Models;

namespace web.api.Services;

public class TodoTableService : ITodoTableService
{
    private readonly StorageConfiguration storageConfig;
    private readonly TableClient tableClient;
    private readonly string partitionKey;

    public TodoTableService(IOptions<StorageConfiguration> config)
    {
        storageConfig = config.Value;
        partitionKey = storageConfig.TableTodo.PartitionKey;

        // Construct a new TableClient using a connection string.
        tableClient = new TableClient(
            storageConfig.ConnectionString,
            storageConfig.TableTodo.TableName);

        // Create the table if it doesn't already exist to verify we've successfully authenticated.
        tableClient.CreateIfNotExists();
    }

    public async Task CompleteItem(string rowKey)
    {
        // Get the entity to update.
        TableEntity qEntity = await tableClient.GetEntityAsync<TableEntity>(partitionKey, rowKey);
        qEntity["IsCompleted"] = !qEntity.GetBoolean(nameof(Todo.IsCompleted));

        // Since no UpdateMode was passed, the request will default to Merge.
        await tableClient.UpdateEntityAsync(qEntity, qEntity.ETag);
    }

    public async Task DeleteItem(string rowKey)
    {
        await tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task<List<Todo>> GetAll()
    {
        return await Task.Run(() =>
        {
            Pageable<Todo> queryResultsLINQ = tableClient.Query<Todo>(ent => ent.PartitionKey == partitionKey);
            return queryResultsLINQ.ToList();
        });
    }

    public async Task<List<Todo>> GetByEmailAsync(string email)
    {
        return await Task.Run(() =>
        {
            Pageable<Todo> queryResultsLINQ = tableClient.Query<Todo>(ent => ent.CreatedByEmail == email);
            return queryResultsLINQ.ToList();
        });
    }

    public async Task<Todo> GetByIdAsync(string rowKey)
    {
        return await Task.Run(() =>
        {
            Pageable<Todo> queryResultsLINQ = tableClient.Query<Todo>(ent => ent.PartitionKey == partitionKey && ent.RowKey == rowKey);
            return queryResultsLINQ.ToList().FirstOrDefault();
        });
    }

    public async Task Upsert(Todo todo)
    {
        try
        {
            var tableEntity = new TableEntity(partitionKey, todo.RowKey);
            tableEntity.Add(nameof(Todo.PartitionKey), partitionKey);
            tableEntity.Add(nameof(Todo.ItemName), todo.ItemName);
            tableEntity.Add(nameof(Todo.IsCompleted), todo.IsCompleted);
            tableEntity.Add(nameof(Todo.CreatedDate), todo.Timestamp);
            tableEntity.Add(nameof(Todo.CreatedByEmail), todo.CreatedByEmail);
            tableEntity.Add(nameof(Todo.CreatedByName), todo.CreatedByName);

            await tableClient.UpsertEntityAsync(tableEntity);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
