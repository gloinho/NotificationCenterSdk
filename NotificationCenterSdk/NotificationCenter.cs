using Microsoft.Extensions.Caching.Memory;
using NotificationCenterSdk.Exceptions;
using NotificationCenterSdk.Interfaces;
using NotificationCenterSdk.Managers;
using NotificationCenterSdk.Models;
using NotificationCenterSdk.Models.Request;
using NotificationCenterSdk.Models.Response;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationCenterSdk
{
    /// <summary>
    /// Classe responsável por realizar as requisições necessárias para autenticação, autorização e envio de notificações.
    /// </summary>
    public class NotificationCenter : INotificationCenter
    {
        private readonly string _authEndpoint = "api/notification/authentication/sign-in";
        private readonly string _sendNotificationEndpoint = "api/notification/send";
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _enginerHttpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly UserCredentials _userCredentials;

        /// <summary>
        /// Inicializa uma nova instancia de <see cref="NotificationCenter"/>.
        /// </summary>
        /// <param name="memoryCache">A instancia de <see cref="IMemoryCache"/> injetada do MemoryCache.</param>
        /// <param name="httpClientFactory">A instancia <see cref="IHttpClientFactory"/> injetada do HttpClientFactory.</param>
        /// <param name="username">O usuário a ser autenticado.</param>
        /// <param name="password">A senha do usuário a ser autenticado</param>
        public NotificationCenter(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, UserCredentials userCredentials)
        {
            _userCredentials = userCredentials;
            _memoryCache = memoryCache;
            _authHttpClient = httpClientFactory.CreateClient("auth");
            _enginerHttpClient = httpClientFactory.CreateClient("enginer");
        }

        /// <summary>
        /// Realiza requisição para o envio de uma notificação. A autenticação do usuário é realizada de maneira automática e 
        /// o Access Token (JWT) é salvo no Memory Cache da aplicação com a key="TOKEN" com validade igual ao key="exp" do JWT.
        /// </summary>
        /// <param name="notification">A instancia da classe <see cref="RequestSendNotification"/> que representa uma notificação a ser serializada e enviada na requisição</param>
        /// <returns>A instancia da classe <see cref="NotificationResponse"/> representando o retorno da Enginer API.</returns>
        /// <exception cref="NotificationException">
        /// Campos de <paramref name="notification"/> inválidos.
        /// </exception>
        /// <exception cref="CredentialsException">
        /// Credenciais inválidas.
        /// </exception>  
        /// <exception cref="AccessTokenException">
        /// Access Token inexistente ou inacessível.
        /// </exception>
        public async Task<NotificationResponse> SendNotification(RequestSendNotification notification)
        {
            string accessToken = await _memoryCache.RetrieveOrCreateAccessToken(_userCredentials, _authEndpoint, _authHttpClient);

            var json = JsonSerializer.Serialize(notification);
            var request = new HttpRequestMessage(HttpMethod.Post, _sendNotificationEndpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            return await RequestToEnginer(request);
        }

        /// <summary>
        /// Realiza requisição para o envio de uma notificação.
        /// </summary>
        /// <param name="notification">A instancia da classe <see cref="RequestSendNotification"/> que representa uma notificação a ser serializada e enviada na requisição</param>
        /// <param name="accessToken">O token JWT para realizar a autenticação no Enginer.</param>
        /// <returns>A instancia da classe <see cref="NotificationResponse"/> representando o retorno da Enginer API.</returns>
        /// <exception cref="NotificationException">
        /// Campos de <paramref name="notification"/> inválidos ou erro de servidor.
        /// </exception>
        /// <exception cref="CredentialsException">
        /// Credenciais inválidas.
        /// </exception>  
        /// <exception cref="AccessTokenException">
        /// Access Token inválido, expirado ou não resgatado.
        /// </exception>
        public async Task<NotificationResponse> SendNotification(RequestSendNotification notification, string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(accessToken))
            {
                throw new AccessTokenException(null, "Access Token inválido.", DateTime.Now);          
            }

            var token = handler.ReadJwtToken(accessToken);

            if (token.ValidTo.ToLocalTime() <= DateTime.Now)
            {
                throw new AccessTokenException(null, "Access Token expirado.", DateTime.Now);
            }

            var json = JsonSerializer.Serialize(notification);
            var request = new HttpRequestMessage(HttpMethod.Post, _sendNotificationEndpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            return await RequestToEnginer(request);

        }

        /// <summary>
        /// Realiza a autenticação do usuário.
        /// </summary>
        /// <returns>O AccessToken(JWT) necessário para realizar o envio de notificação para o 
        /// <see cref="SendNotification(RequestSendNotification, string)"/></returns>
        public async Task<string> Authenticate()
        {
            var tokenModel = await UserAuthenticationManager.FetchAccessToken(_userCredentials, _authEndpoint, _authHttpClient);
            return tokenModel.Value;
        }

        private async Task<NotificationResponse> RequestToEnginer(HttpRequestMessage request)
        {
            try
            {
                var response = await _enginerHttpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => JsonSerializer.Deserialize<NotificationResponse>(content),
                    HttpStatusCode.BadRequest => throw JsonSerializer.Deserialize<NotificationException>(content),
                    HttpStatusCode.InternalServerError => throw JsonSerializer.Deserialize<NotificationException>(content),
                    _ => throw new NotificationException(null, "Não foi possível identificar a resposta da API.", DateTime.Now)
                };
            }
            catch (HttpRequestException httpRequestException)
            {
                throw new NotificationException(HttpStatusCode.InternalServerError,
                    $"Não foi possível enviar a notificação: {httpRequestException.Message}",
                    DateTime.Now,
                    httpRequestException.InnerException?.Message,
                    _sendNotificationEndpoint);
            }
        }
    }
}
