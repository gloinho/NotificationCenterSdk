using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace NotificationCenterSdk.Exceptions
{
    /// <summary>
    /// Exceção personalizada para representar erros relacionados a notificações.
    /// </summary>
    [Serializable]
    public class NotificationException : Exception
    {
        /// <summary>
        /// Obtém ou define o código de status HTTP associado à exceção, se aplicável.
        /// </summary>
        [JsonPropertyName("statusCode")]
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Obtém ou define a mensagem de erro da exceção.
        /// </summary>
        [JsonPropertyName("message")]
        public List<string?> Message { get; set; }

        /// <summary>
        /// Obtém ou define detalhes adicionais relacionados à exceção, se aplicável.
        /// </summary>
        [JsonPropertyName("detail")]
        public string? Detail { get; set; }

        /// <summary>
        /// Obtém ou define a data e hora em que a exceção ocorreu.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Obtém ou define o caminho ou rota relacionado à exceção, se aplicável.
        /// </summary>
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        /// <summary>
        /// Inicializa uma nova instância da classe NotificationException com as informações fornecidas.
        /// </summary>
        /// <param name="statusCode">O código de status HTTP associado à exceção.</param>
        /// <param name="message">A mensagem de erro da exceção.</param>
        /// <param name="timeStamp">A data e hora em que a exceção ocorreu.</param>
        /// <param name="detail">Detalhes adicionais relacionados à exceção.</param>
        /// <param name="path">O caminho ou rota relacionado à exceção.</param>
        public NotificationException(HttpStatusCode? statusCode, string message, DateTime timeStamp, string? detail = null, string? path = null)
        {
            StatusCode = statusCode;
            Message = new List<string?> { message };
            Detail = detail;
            TimeStamp = timeStamp;
            Path = path;
        }
    }
}
