using FluentValidation;

namespace App.Business.Features.MoneyManager.UploadSpreadsheet
{
    public class UploadSpreadsheetCommandValidator : AbstractValidator<UploadSpreadsheetCommand>
    {
        public UploadSpreadsheetCommandValidator()
        {
            RuleFor(x => x.BlobName)
                .NotEmpty()
                .WithMessage("File name is required.")
                .Matches(@"^[a-zA-Z0-9_\-\.]+$")
                .WithMessage("File name can only contain letters, numbers, underscores, dashes, and dots.");

            RuleFor(x => x.Container)
                .NotEmpty()
                .WithMessage("File type is required.");
        }
    }
}
