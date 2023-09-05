using Microsoft.IdentityModel.Tokens;
using RaroNotifications.Exceptions;
using System;
using System.Text.Json.Serialization;

namespace RaroNotifications.Models.Request
{
    /// <summary>
    /// Modelo de receivers enviados para o Enginer API para envio de notificação. Pelo menos um deverá existir./>.
    /// </summary>
    public class RequestReceiverSendNotification
    {
        /// <summary>
        /// Inicializa uma nova instancia de <see cref="RequestReceiverSendNotification"/>.
        /// </summary>
        /// <param name="identificadorUsuario">Id do usuário a receber a notificação.</param>
        /// <param name="deviceId">Id do Aparelho a receber a notificação.</param>
        /// <param name="phone">Número a receber a notificação.</param>
        /// <param name="email">Email a receber notificação.</param>
        public RequestReceiverSendNotification(string? identificadorUsuario, string? deviceId, string? phone, string? email)
        {
            IdentificadorUsuario = identificadorUsuario;
            DeviceId = deviceId;
            Phone = phone;
            Email = email;

            if (IsNotValidModel())
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