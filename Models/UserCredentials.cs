using System.Text.Json.Serialization;
namespace RaroNotifications.Models
{
    public class UserCredentials
    {
        [JsonPropertyName("userName")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
