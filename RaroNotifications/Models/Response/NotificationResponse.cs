using System;
using System.Text.Json.Serialization;

namespace RaroNotifications.Models.Response
{
    public class NotificationResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("error")]
        public ErrorResponse? Error { get; set; }
    }
}
