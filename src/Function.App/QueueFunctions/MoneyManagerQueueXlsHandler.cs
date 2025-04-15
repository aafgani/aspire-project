using App.Domain.Features.MoneyManager.UploadSpreadsheet;
using App.Domain.Interface;
using App.Function.Configurations;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace App.Function.QueueFunctions
{
    public class MoneyManagerQueueXlsHandler
    {
        private readonly ILogger<MoneyManagerQueueXlsHandler> _logger;
        private readonly ICommandDispatcher dispatcher;

        public MoneyManagerQueueXlsHandler(ILogger<MoneyManagerQueueXlsHandler> logger, ICommandDispatcher dispatcher)
        {
            _logger = logger;
            this.dispatcher = dispatcher;
        }

        [Function(FunctionsName.MoneyManagerQueueXlsHandler)]
        public async Task RunAsync([QueueTrigger("money-manager", Connection = "")] QueueMessage message, 
            FunctionContext context)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            var command = JsonSerializer.Deserialize<UploadSpreadsheetCommand>(message.MessageText);

            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var result = await dispatcher.DispatchAsync(command);


        }
    }
}
