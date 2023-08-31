using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using RaroNotifications.Exceptions;
using RaroNotifications.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;

namespace RaroNotifications
{
    public static class TokenHandler
    {
        /// <summary>
        /// Recupera o Access Token (JWT) do Memory Cache.
        /// </summary>
        /// <param name="user">A instancia de <see cref="User"/> que representa o usuário a ser autenticado na rota de autenticação.</param>
        /// <param name="authUrl">A rota de autenticação da Customer Api.</param>
        /// <returns>O token JWT necessário para efetuar o envio de uma notificação.</returns>
        /// <exception cref="NotificationException">
        /// <paramref name="isbn"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="isbn"/> is an empty or white-space string.
        /// </exception>
        public static async Task<string> GetAccessToken(this IMemoryCache memoryCache, User user, string authUrl)
        {
            var token = memoryCache.Get<string>("TOKEN");
            if (token != null)
            {
                return token;
            }

            var tokenModel = await FetchAccessToken(user, authUrl);

            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(tokenModel.ValidTo);
            memoryCache.Set("TOKEN", tokenModel.Value, options);
            return tokenModel.Value;
        }

        private static async Task<TokenModel> FetchAccessToken(User user, string authUrl)
        {
            using var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, authUrl);

            var credentials = JsonSerializer.Serialize(user);

            request.Content = new StringContent(credentials, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.SendAsync(request);
                string accessToken = string.Empty;

                if (response.Headers.TryGetValues("Set-Cookie", out var cookieValues))
                {
                    var cookies = SetCookieHeaderValue.ParseList(cookieValues.ToList());
                    var accessTokenCookie = cookies.FirstOrDefault(cookie => cookie.Name == "access_token");
                    if (accessTokenCookie != null)
                    {
                        accessToken = accessTokenCookie.Value.ToString();
                    }
                }
                else
                {
                    throw new NotificationException(HttpStatusCode.BadRequest, $"Wrong Credentials: Access Token error. Url: {authUrl} ", DateTime.Now, path:authUrl);
                }

                var handler = new JwtSecurityTokenHandler();

                var validTo = handler.ReadJwtToken(accessToken).ValidTo.ToLocalTime();

                return new TokenModel { Value = accessToken, ValidTo = validTo };

            }
            catch (HttpRequestException ex)
            {
                throw new NotificationException(ex.StatusCode, ex.Message ,DateTime.Now);
            }
        }
    }
}
