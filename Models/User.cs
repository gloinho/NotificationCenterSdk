using System.Text.Json.Serialization;
namespace RaroNotifications.Models
{
    internal class User
    {
        [JsonPropertyName("userName")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
