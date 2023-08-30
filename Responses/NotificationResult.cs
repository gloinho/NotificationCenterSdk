using System.Text.Json.Serialization;

namespace RaroNotifications.Responses
{
    public class NotificationResult
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
    }
}
