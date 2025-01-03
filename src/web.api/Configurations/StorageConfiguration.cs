namespace web.api.Configurations;

public class StorageConfiguration
{
    public string ConnectionString { get; set; }
    public TableConfiguration TableTodo { get; set; }
}
public class TableConfiguration
{
    public string TableName { get; set; }
    public string PartitionKey { get; set; }
}