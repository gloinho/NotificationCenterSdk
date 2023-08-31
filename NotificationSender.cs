using Microsoft.Extensions.Caching.Memory;
using RaroNotifications.Exceptions;
using RaroNotifications.Handlers;
using RaroNotifications.Models;
using RaroNotifications.Models.RequestModels;
using RaroNotifications.Responses;
using System.Net;
using System.Text;
using System.Text.Json;

namespace RaroNotifications
{
    /// <summary>
    /// Classe responsável por realizar as requisições necessárias para autenticação, autorização e envio de notificações.
    /// </summary>
    public class NotificationSender
    {
        private readonly string _authEndpoint = ":3001/api/notification/authentication/sign-in";
        private readonly string _sendNotificationEndpoint = ":3003/api/notification/send";
        private User User { get; set; }
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Inicializa uma nova instancia de <see cref="NotificationSender"/>.
        /// </summary>
        /// <param name="memoryCache">A <see cref="IMemoryCache"/> instancia injetada do MemoryCache da aplicação.</param>
        /// <param name="baseUrl">A URL base a ser utilizada.</param>
        /// <param name="username">O usuário a ser autenticado.</param>
        /// <param name="password">A senha do usuário a ser autenticado</param>
        public NotificationSender(IMemoryCache memoryCache, string baseUrl, string username, string password)
        {
            _authEndpoint = baseUrl + _authEndpoint;
            _sendNotificationEndpoint = baseUrl + _sendNotificationEndpoint;
            User = new User { Username = username, Password = password };
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Realiza requisição para o envio de uma notificação.
        /// </summary>
        /// <param name="notification">A instancia da classe <see cref="RequestSendNotification"/> que representa uma notificação a ser serializada e enviada na requisição</param>
        /// <returns>A instancia da classe <see cref="NotificationResponse"/> representando o retorno da Enginer API.</returns>
        /// <exception cref="NotificationException">
        /// Campos de <paramref name="notification"/> inválidos.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// Não foi possivel realizar a requisição.
        /// </exception>   
        /// <exception cref="CredentialsException">
        /// Credenciais inválidas.
        /// </exception>  
        /// <exception cref="AccessTokenException">
        /// Access Token inexistente ou inacessível.
        /// </exception>
        public async Task<NotificationResponse> SendNotification(RequestSendNotification notification)
        {
            string accessToken = await _memoryCache.GetAccessToken(User, _authEndpoint);

            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, _sendNotificationEndpoint);

            var json = JsonSerializer.Serialize(notification);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            try
            {
                var response = await client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                return response.StatusCode switch
                {
                    HttpStatusCode.Created => JsonSerializer.Deserialize<NotificationResponse>(content),
                    HttpStatusCode.BadRequest => throw JsonSerializer.Deserialize<NotificationException>(content),
                    _ => null,
                };
            }
            catch (HttpRequestException httpRequestException)
            {
                throw httpRequestException;
            }
        }
    }
}