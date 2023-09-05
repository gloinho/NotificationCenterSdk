using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NotificationCenterSdk.Models.Response
{
    public class ErrorResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("stack")]
        public string? Stack { get; set; }
    }
}
