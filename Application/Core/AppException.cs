namespace Application.Core
{
    public class AppException
    {
        //give details default value of null. details will be the stacktrace that we return
        public AppException(int statusCode, string message, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}