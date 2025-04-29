using App.Domain.Model.Dtos.Customer;
using App.Domain.Model.Dtos.Pagination;
using App.Domain.Model.QueryParams;

namespace App.Domain.Interface
{
    public interface ICustomerService
    {
        Task<PagedResult<CustomerDto>> GetAllCustomers(CustomerQueryParams queryParams);
    }
}
