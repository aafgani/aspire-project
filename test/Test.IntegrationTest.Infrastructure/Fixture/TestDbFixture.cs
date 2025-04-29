using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Test.IntegrationTest.Infrastructure.Helpers;
using Testcontainers.MsSql;

namespace Test.IntegrationTest.Infrastructure.Fixture
{
    public class TestDbFixture : IAsyncLifetime
    {

        private const string DbName = "TestDb";
        public MsSqlContainer MsSqlContainer { get; private set; } = null!;
        public string ConnectionString => SanitizeConnectionString(MsSqlContainer.GetConnectionString());

        private string SanitizeConnectionString(string connectionString)
        {
            return connectionString
                .Replace("localhost", "127.0.0.1")
                .Replace("master", DbName);
        }

        public ChinookDb CreateContext(string connectionString)
        => new(
            new DbContextOptionsBuilder<ChinookDb>()
                .UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.CommandTimeout(60);
                })
                .Options);

        public async Task DisposeAsync()
        {
            await MsSqlContainer.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            MsSqlContainer = new MsSqlBuilder()
             .WithImage("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
             .WithPassword("Password1234!")
             .Build();

            await MsSqlContainer.StartAsync().ConfigureAwait(false);

            await CreateDatabase();

            // Initialize the database.
            await using var context = CreateContext(ConnectionString);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            await context.SaveChangesAsync();
        }

        private async Task CreateDatabase()
        {
            var dbConnectionFactory = new DbConnectionFactory(MsSqlContainer.GetConnectionString().Replace("localhost", "127.0.0.1"), DbName);
            await using var connection = dbConnectionFactory.MasterDbConnection;

            await using var command = connection.CreateCommand();
            command.CommandText = "CREATE DATABASE " + DbName;
            await connection.OpenAsync()
                .ConfigureAwait(false);

            await command.ExecuteNonQueryAsync()
                .ConfigureAwait(false);
        }
    }
}
