using Microsoft.IdentityModel.Tokens;
using NotificationCenterSdk.Exceptions;
using System;
using System.Text.Json.Serialization;

namespace NotificationCenterSdk.Models.Request
{
    /// <summary>
    /// Modelo de destinatários enviados para a Enginer API para envio de notificações. Pelo menos um destinatário deve existir.
    /// </summary>
    public class RequestReceiverSendNotification
    {
        /// <summary>
        /// Inicializa uma nova instância da classe RequestReceiverSendNotification.
        /// </summary>
        /// <param name="identificadorUsuario">O ID do usuário que receberá a notificação.</param>
        /// <param name="deviceId">O ID do dispositivo que receberá a notificação.</param>
        /// <param name="phone">O número de telefone que receberá a notificação.</param>
        /// <param name="email">O endereço de e-mail que receberá a notificação.</param>
        /// <exception cref="NotificationException">
        /// Lançada se todos os parâmetros forem nulos ou vazios.
        /// </exception>
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
        /// <summary>
        /// Obtém ou define o ID do usuário que receberá a notificação.
        /// </summary>
        [JsonPropertyName("identificadorUsuario")]
        public string? IdentificadorUsuario { get; set; }

        /// <summary>
        /// Obtém ou define o ID do dispositivo que receberá a notificação.
        /// </summary>
        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }

        /// <summary>
        /// Obtém ou define o número de telefone que receberá a notificação.
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Obtém ou define o endereço de e-mail que receberá a notificação.
        /// </summary>
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