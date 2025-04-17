using App.Domain.Interface;
using App.Domain.Model.QueryParams;

namespace Web.API.Endpoints
{
    public static class CustomerEndpoints
    {
        public static RouteGroupBuilder MapCustomers(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup(EndpointGroupNames.CustomersGroupName);

            group.MapGet("/all", async (ICustomerService service, [AsParameters] CustomerQueryParams query) =>
            {
                var result = await service.GetAllCustomers(query);
                return result is null ? Results.NotFound() : Results.Ok(result);
            })
                .WithName("GetAllCustomers")
                .WithSummary("Retrieve customers with optional filtering and pagination.")
                .WithDescription("Returns a paginated list of customers. You can optionally filter by country."); 

            return group;
        }
    }
}
