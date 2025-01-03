using Azure;
using Azure.Data.Tables;

namespace web.api.Models;

public class Todo : ITableEntity
{
    public string ItemName { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByEmail { get; set; }
    public string CreatedByName { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
