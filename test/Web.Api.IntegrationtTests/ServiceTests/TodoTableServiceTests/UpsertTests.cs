using Microsoft.Extensions.Options;
using web.api.Configurations;
using web.api.Services;

namespace Web.Api.IntegrationtTests.ServiceTests.TodoTableServiceTests;

public class UpsertTests
{
    private readonly TodoTableService todoTableService;

    public UpsertTests(IOptions<StorageConfiguration> config)
    {
        todoTableService = new TodoTableService(config);
    }
}
