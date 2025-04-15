using App.Domain.Interface;

namespace App.Domain.Features.MoneyManager.UploadSpreadsheet
{
    public class UploadSpreadsheetCommand : ICommand
    {
        public string Container { get; }
        public string BlobName { get; }
        public UploadSpreadsheetCommand(string container, string blobName)
        {
            Container = container;
            BlobName = blobName;
        }
    }
}
