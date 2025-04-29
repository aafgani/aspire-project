using App.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Test.IntegrationTest.Api.Fixture
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private static readonly SemaphoreSlim _semaphore = new(1, 1);
        protected DatabaseContainerFixture _dbFixture = new();

        public async Task InitializeAsync()
        {
            await _dbFixture.InitializeAsync();
        }
        public async Task DisposeAsync()
        {
            await _dbFixture.DisposeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ChinookDb>));

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<ChinookDb>(options =>
                {
                    options.UseSqlServer(_dbFixture.ConnectionString);
                });
            });
        }

        private string GetThreadSafeConnectionString()
        {
            _semaphore.Wait();
            try
            {
                return _dbFixture.ConnectionString;
            }
            finally
            {
                _semaphore.Release();
            }
        }

    }
}
