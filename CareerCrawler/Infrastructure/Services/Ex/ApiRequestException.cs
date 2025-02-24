namespace HHParser.Infrastructure.Services.Ex
{
    public class ApiRequestException : Exception
    {
        public ApiRequestException() { }

        public ApiRequestException(string message) : base(message) { }

        public ApiRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
