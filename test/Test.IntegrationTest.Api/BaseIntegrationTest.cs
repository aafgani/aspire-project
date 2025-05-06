using App.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Test.IntegrationTest.Api.Fixture;

namespace Test.IntegrationTest.Api
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly HttpClient Client;
        protected readonly ChinookDb ChinookDbContext;   

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();
            ChinookDbContext = _scope.ServiceProvider.GetRequiredService<ChinookDb>();
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost") // optional
            });
        }
    }
}
