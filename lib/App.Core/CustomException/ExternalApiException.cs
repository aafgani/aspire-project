namespace App.Domain.CustomException
{
    public class ExternalApiException : Exception
    {
        public ExternalApiException(string message) 
            : base("One or more validation errors occurred.") 
        { 
        }
    }
}
