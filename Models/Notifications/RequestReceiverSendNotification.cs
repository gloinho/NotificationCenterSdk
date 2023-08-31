using Microsoft.IdentityModel.Tokens;
using RaroNotifications.Exceptions;
using System.Text.Json.Serialization;

namespace RaroNotifications.Models.Notifications
{
    public class RequestReceiverSendNotification
    {
        public RequestReceiverSendNotification(string? identificadorUsuario, string? deviceId, string? phone, string? email)
        {
            IdentificadorUsuario = identificadorUsuario;
            DeviceId = deviceId;
            Phone = phone;
            Email = email;

            if(IsNotValidModel())
            {
                throw new NotificationException(null, 
                    $"{nameof(RequestReceiverSendNotification)}: precisa de pelo menos uma propriedade preenchida.",
                    DateTime.Now);
            };
        }

        [JsonPropertyName("identificadorUsuario")]
        public string? IdentificadorUsuario { get; set; }
        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        private bool IsNotValidModel()
        {
            return 
                IdentificadorUsuario.IsNullOrEmpty() &&
                DeviceId.IsNullOrEmpty() &&
                Phone.IsNullOrEmpty() &&
                Email.IsNullOrEmpty();
        }
    }
}