namespace API.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; }
        public string Message { get; }
        public string? Details { get; }

        public ApiErrorResponse(int statusCode, string message, string? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
