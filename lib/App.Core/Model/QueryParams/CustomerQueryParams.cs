using System.ComponentModel;

namespace App.Domain.Model.QueryParams
{
    public class CustomerQueryParams
    {
        public string? SortBy { get; set; } = "Name"; // e.g., Name, Country, etc.
        public string? SortDirection { get; set; } = "asc";

        public string? Country { get; set; }
        
        [DefaultValue(1)]
        public int Page { get; set; } = 1;

        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
    }
}
