using Microsoft.Extensions.Caching.Memory;
using RaroNotifications.Exceptions;
using RaroNotifications.Interfaces;
using RaroNotifications.Manager;
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
    public class NotificationSender : INotificationSender
    {
        private readonly string _authEndpoint = "api/notification/authentication/sign-in";
        private readonly string _sendNotificationEndpoint = "api/notification/send";
        private readonly HttpClient _customerHttpClient;
        private readonly HttpClient _enginerHttpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly UserCredentials _userCredentials;

        /// <summary>
        /// Inicializa uma nova instancia de <see cref="NotificationSender"/>.
        /// </summary>
        /// <param name="memoryCache">A instancia de <see cref="IMemoryCache"/> injetada do MemoryCache.</param>
        /// <param name="httpClientFactory">A instancia <see cref="IHttpClientFactory"/> injetada do HttpClientFactory.</param>
        /// <param name="username">O usuário a ser autenticado.</param>
        /// <param name="password">A senha do usuário a ser autenticado</param>
        public NotificationSender(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, UserCredentials userCredentials)
        {
            _userCredentials = userCredentials;
            _memoryCache = memoryCache;
            _customerHttpClient = httpClientFactory.CreateClient("customer");
            _enginerHttpClient = httpClientFactory.CreateClient("enginer");
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
            string accessToken = await _memoryCache.RetrieveOrCreateAccessToken(_userCredentials, _authEndpoint, _customerHttpClient);
            var request = new HttpRequestMessage(HttpMethod.Post, _sendNotificationEndpoint);
            var json = JsonSerializer.Serialize(notification);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            try
            {
                var response = await _enginerHttpClient.SendAsync(request);

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
