using RaroNotifications.Models;
using System.Net;

namespace RaroNotifications.Exceptions
{
    public class CredentialsException : Exception
    {
        public CredentialsException(User user, HttpStatusCode statusCode, string message)
        {
            User = user;
            StatusCode = statusCode;

        }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}
