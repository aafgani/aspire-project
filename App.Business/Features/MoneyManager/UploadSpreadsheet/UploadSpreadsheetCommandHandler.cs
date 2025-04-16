using App.Domain.Interface;
using App.Domain.Model;
using Microsoft.Extensions.Logging;

namespace App.Business.Features.MoneyManager.UploadSpreadsheet
{
    public class UploadSpreadsheetCommandHandler : ICommandHandler<UploadSpreadsheetCommand>
    {
        private readonly ILogger<UploadSpreadsheetCommandHandler> _logger;

        public UploadSpreadsheetCommandHandler(ILogger<UploadSpreadsheetCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task<Result> HandleAsync(UploadSpreadsheetCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Handling UploadSpreadsheetCommand: FileType={command.Container}, BlobName={command.BlobName}");
            return Task.FromResult(Result.Success());
        }
    }
}
