using App.Function.Configurations;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace App.Function.QueueFunctions
{
    public class MoneyManagerQueueXlsHandler
    {
        private readonly ILogger<MoneyManagerQueueXlsHandler> _logger;

        public MoneyManagerQueueXlsHandler(ILogger<MoneyManagerQueueXlsHandler> logger)
        {
            _logger = logger;
        }

        [Function(FunctionsName.MoneyManagerQueueXlsHandler)]
        public void Run([QueueTrigger("money-manager", Connection = "")] QueueMessage message, 
            FunctionContext context)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
