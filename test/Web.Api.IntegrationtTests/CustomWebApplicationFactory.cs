using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using web.api.Configurations;
using web.api.Services;

namespace Web.Api.IntegrationtTests;

internal class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables() // For CI/CD.
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true) // For local development.
                .Build();

        builder.ConfigureTestServices(services =>
        {
            services
                .Configure<StorageConfiguration>(configuration.GetSection("StorageConfiguration"));

            services.AddSingleton<ITodoTableService, TodoTableService>();
        });
    }
}
