using System;
using System.Net;

namespace NotificationCenterSdk.Exceptions
{
    public class CredentialsException : NotificationException
    {
        public CredentialsException(HttpStatusCode? statusCode, string message, DateTime timeStamp, string? detail = null, string? path = null) 
            :base(statusCode, message, timeStamp, detail, path)
        {
        }
    }
}
