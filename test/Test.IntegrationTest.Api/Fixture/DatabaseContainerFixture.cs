using App.Domain.Entities;
using App.Infrastructure.Data;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Net.Sockets;
using Test.IntegrationTest.Api.Helpers;
using Testcontainers.MsSql;

namespace Test.IntegrationTest.Api.Fixture
{
    public class DatabaseContainerFixture : IAsyncLifetime
    {
        private const string DbName = "TestDb";
        private const string SaPassword = "Password1234!";
        private const int ContainerPort = 1433;
        public MsSqlContainer _dbContainer { get; private set; } = null!;
        public string ConnectionString => SanitizeConnectionString(_dbContainer.GetConnectionString());
        private string SanitizeConnectionString(string connectionString)
        {
            return connectionString
                .Replace("localhost", "127.0.0.1")
                .Replace("master", DbName);
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            _dbContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
                .WithPortBinding(ContainerPort, true)
                .WithPassword(SaPassword)
                .Build();

            await _dbContainer.StartAsync();

            var hostPort = _dbContainer.GetMappedPublicPort(ContainerPort);
            var connectionString = $"Server=127.0.0.1,{hostPort};Database={DbName};User Id=sa;Password={SaPassword};TrustServerCertificate=True";

            // 👇 Wait until SQL Server inside the container is reachable
            await WaitUntilDatabaseAvailableAsync("127.0.0.1", hostPort);

            await CreateDatabase();

            var options = new DbContextOptionsBuilder<ChinookDb>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new ChinookDb(options))
            {
                await context.Database.EnsureCreatedAsync();
                await SeedDataAsync(context);
            }
        }

        private async Task WaitUntilDatabaseAvailableAsync(string host, int port)
        {
            var retries = 10;
            var delay = TimeSpan.FromSeconds(2);

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    using var tcpClient = new TcpClient();
                    await tcpClient.ConnectAsync(host, port);
                    if (tcpClient.Connected)
                    {
                        Console.WriteLine($"✅ Database server available at {host}:{port}");
                        return;
                    }
                }
                catch
                {
                    Console.WriteLine($"⏳ Waiting for database server at {host}:{port}...");
                    // Ignore and retry
                }
                await Task.Delay(delay);
            }
            throw new Exception($"❌ Could not connect to the database server at {host}:{port} within the time limit.");
        }

        private Task SeedDataAsync(ChinookDb context)
        {
            return Task.CompletedTask;
        }

        private async Task CreateDatabase()
        {
            var dbConnectionFactory = new DbConnectionFactory(_dbContainer.GetConnectionString()
                .Replace("localhost", "127.0.0.1"), DbName);

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
