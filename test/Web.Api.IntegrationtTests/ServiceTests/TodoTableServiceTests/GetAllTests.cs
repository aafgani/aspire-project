using Microsoft.Extensions.Options;
using web.api.Configurations;
using web.api.Services;

namespace Web.Api.IntegrationtTests.ServiceTests;

public class GetAllTests
{
    private readonly TodoTableService todoTableService;

    public GetAllTests(IOptions<StorageConfiguration> config)
    {
        todoTableService = new TodoTableService(config);
    }

    [Fact]
    public async Task GivenNoFilter_GetAll_ShouldReturnAllTodo()
    {

    }
}
