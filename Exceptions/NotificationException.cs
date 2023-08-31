﻿using System.Net;
using System.Text.Json.Serialization;

namespace RaroNotifications.Exceptions
{
    [Serializable]
    public class NotificationException : Exception
    {
        public NotificationException() 
        {
            
        }
        public NotificationException(HttpStatusCode? statusCode,string message, DateTime timeStamp, string? detail = null, string? path=null) : base(message)
        {
            StatusCode = statusCode;
            Message = new List<string?> { message };
            Detail = detail;
            TimeStamp = timeStamp;
            Path = path;
        }

        [JsonPropertyName("statusCode")]
        public HttpStatusCode? StatusCode { get; set; }
        [JsonPropertyName("message")]
        public List<string?> Message { get; set; }
        [JsonPropertyName("detail")]
        public string? Detail { get; set; }
        [JsonPropertyName("timeStamp")]
        public DateTime TimeStamp { get; set; }
        [JsonPropertyName("path")]
        public string? Path { get; set; }
    }
}