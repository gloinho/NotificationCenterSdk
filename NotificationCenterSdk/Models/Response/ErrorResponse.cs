using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NotificationCenterSdk.Models.Response
{
    /// <summary>
    /// Representa informações de erro em uma resposta da Enginer API.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Obtém ou define o nome do erro.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Obtém ou define a mensagem de erro detalhada.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Obtém ou define a pilha de chamadas ou rastreamento de erro, se disponível.
        /// </summary>
        [JsonPropertyName("stack")]
        public string? Stack { get; set; }
    }
}