namespace App.Domain.CustomException
{
    public class DomainValidationException : Exception
    {
        public IReadOnlyList<string> Errors { get; }
        public DomainValidationException(IEnumerable<string> errors)
        : base("One or more validation errors occurred.")
        {
            Errors = errors.ToList();
        }
    }
}
