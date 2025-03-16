using App.Function.Configurations;
using App.Infrastructure.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Function
{
    public class InitializationService : IHostedService
    {
        private readonly ILogger<InitializationService> _logger;
        private readonly string _azureStorageConnectionString;

        public InitializationService(IOptions<ConnectionStringOptions> connectionStringOptions, ILogger<InitializationService> logger)
        {
            _logger = logger;
            _azureStorageConnectionString = connectionStringOptions.Value.AzureStorage; 
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting initialization checks prior to the function starting ...");

            try
            {
                var queueClientFacrory = new QueueClientFactory(_azureStorageConnectionString);
                var moneymanagerQueueClient = queueClientFacrory.CreateClient("money-manager");

                var tasks = new Task[]
               {
                    moneymanagerQueueClient.CreateIfNotExistsAsync(null, cancellationToken)
               };

                await Task.WhenAll(tasks);

                _logger.LogInformation(" ... done!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to initialize any initialization-startup logic.");
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
