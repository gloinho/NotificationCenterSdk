using System.Net;

namespace RaroNotifications.Exceptions
{
    public class CredentialsException : Exception
    {
        public CredentialsException(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;

        }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
