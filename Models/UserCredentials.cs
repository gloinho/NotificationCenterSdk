using System.Text.Json.Serialization;
namespace RaroNotifications.Models
{
    /// <summary>
    /// Classe responsável por armazenar as credenciais do usuário. Configurada por DI./>
    /// </summary>
    public class UserCredentials
    {
        [JsonPropertyName("userName")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
