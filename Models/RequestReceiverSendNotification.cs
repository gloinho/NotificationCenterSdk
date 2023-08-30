using System.Text.Json.Serialization;

namespace RaroNotifications.Models
{
    public class RequestReceiverSendNotification
    {
        public RequestReceiverSendNotification(string? identificadorUsuario, string? deviceId, string? phone, string? email)
        {
            IdentificadorUsuario = identificadorUsuario;
            DeviceId = deviceId;
            Phone = phone;
            Email = email;
        }

        [JsonPropertyName("identificadorUsuario")]
        public string? IdentificadorUsuario { get; set; }
        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }
}