using Microsoft.Net.Http.Headers;
using RaroNotifications.Exceptions;
using RaroNotifications.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text;

namespace RaroNotifications.Manager
{
    internal class UserAuthenticationManager
    {
        internal static async Task<AccessTokenModel> FetchAccessToken(UserCredentials userCredentials, 
            string authUrl, HttpClient httpClient)
        {
            string accessToken = string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Post, authUrl);
            var credentials = JsonSerializer.Serialize(userCredentials);
            request.Content = new StringContent(credentials, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStreamAsync();
                    throw JsonSerializer.Deserialize<CredentialsException>(error);
                }

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
                    var error = await response.Content.ReadAsStreamAsync();
                    throw JsonSerializer.Deserialize<AccessTokenException>(error);
                }

                var handler = new JwtSecurityTokenHandler();

                var validTo = handler.ReadJwtToken(accessToken).ValidTo.ToLocalTime();

                return new AccessTokenModel(accessToken, validTo);

            }
            catch (HttpRequestException httpException)
            {
                throw new AccessTokenException(httpException.StatusCode,$"Falha ao buscar access token: {httpException.Message}", DateTime.Now);
            }
        }
    }
}
