using System.ComponentModel.DataAnnotations;

namespace App.Function.Configurations
{
    public class ConnectionStringOptions
    {
        public const string ConfigurationSectionKey = "ConnectionStrings";

        public string SqlServer { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        public string AzureStorage { get; set; } = default!;
    }
}
