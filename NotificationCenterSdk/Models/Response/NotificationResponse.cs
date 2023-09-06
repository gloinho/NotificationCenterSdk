using System;
using System.Text.Json.Serialization;

namespace NotificationCenterSdk.Models.Response
{
    /// <summary>
    /// Representa a resposta de uma solicitação de envio de notificação feita à Enginer API.
    /// </summary>
    public class NotificationResponse
    {
        /// <summary>
        /// Obtém ou define o ID único associado à notificação.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Obtém ou define a data e hora em que a notificação foi processada pela Enginer API.
        /// </summary>
        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Obtém ou define um valor booleano que indica se a notificação foi enviada com sucesso.
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Obtém ou define informações sobre erros, se houver, durante o envio da notificação.
        /// </summary>
        [JsonPropertyName("error")]
        public ErrorResponse? Error { get; set; }
    }
}
