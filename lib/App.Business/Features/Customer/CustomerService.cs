using App.Business.Mapper;
using App.Domain.Entities;
using App.Domain.Interface;
using App.Domain.Model.Dtos.Customer;
using App.Domain.Model.Dtos.Pagination;
using App.Domain.Model.QueryParams;
using App.Infrastructure.Data;
using Azure;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Business.Features.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ChinookDb _db;
        private readonly CustomerMapper _mapper;

        public CustomerService(ChinookDb db, CustomerMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<PagedResult<CustomerDto>> GetAllCustomers(CustomerQueryParams queryParams)
        {
            var query = _db.Customers.AsQueryable();
            
            if (!string.IsNullOrEmpty(queryParams.Country))
            {
                query = query.Where(c => c.Country == queryParams.Country);
            }

            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                query = queryParams.SortDirection?.ToLower() switch
                {
                    "desc" => queryParams.SortBy.ToLower() switch
                    {
                        "name" => query.OrderByDescending(c => c.LastName),
                        "country" => query.OrderByDescending(c => c.Country),
                        _ => query.OrderByDescending(c => c.LastName)
                    },
                    "asc" => queryParams.SortBy.ToLower() switch
                    {
                        "name" => query.OrderBy(c => c.LastName),
                        "country" => query.OrderBy(c => c.Country),
                        _ => query.OrderBy(c => c.LastName)
                    },
                };
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)queryParams.PageSize);

            var customers = await query
                .OrderBy(c => c.LastName)
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            var result = customers.Select(_mapper.ToDto);

            return new PagedResult<CustomerDto>
            {
                Data = customers.Select(_mapper.ToDto),
                Pagination = new PaginationMetadata
                {
                    CurrentPage = queryParams.Page,
                    PageSize = queryParams.PageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                }
            };
        }
    }
}
