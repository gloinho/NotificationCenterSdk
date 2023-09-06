using System.Text.Json.Serialization;

namespace NotificationCenterSdk.Models
{
    /// <summary>
    /// Representa as credenciais do usuário necessárias para autenticação na Enginer API.
    /// Essas credenciais são configuradas por meio da injeção de dependência (DI).
    /// </summary>
    public class UserCredentials
    {
        /// <summary>
        /// Obtém ou define o nome de usuário do usuário para autenticação.
        /// </summary>
        [JsonPropertyName("userName")]
        public string Username { get; set; }

        /// <summary>
        /// Obtém ou define a senha do usuário para autenticação.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}