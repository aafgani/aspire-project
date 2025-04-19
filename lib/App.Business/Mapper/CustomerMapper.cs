using App.Domain.Entities;
using App.Domain.Model.Dtos.Customer;
using Riok.Mapperly.Abstractions;

namespace App.Business.Mapper
{
    [Mapper]
    public partial class CustomerMapper
    {
        public partial CustomerDto ToDto(Customer customer);
    }
}
