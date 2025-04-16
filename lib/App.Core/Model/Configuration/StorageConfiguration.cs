using System.ComponentModel.DataAnnotations;

namespace App.Domain.Model.Configuration
{
    public class StorageConfiguration
    {
        [Required(ErrorMessage = "Storage ConnectionString required")]
        public string ConnectionString { get; set; }
        public TableConfiguration TableTodo { get; set; }
    }
    public class TableConfiguration
    {
        public string TableName { get; set; }
        public string PartitionKey { get; set; }
    }
}
