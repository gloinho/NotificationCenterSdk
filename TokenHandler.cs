using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using RaroNotifications.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;

namespace RaroNotifications
{
    public static class TokenHandler
    {
        public static async Task<string?> GetAccessToken(this IMemoryCache memoryCache, User user, string authUrl)
        {
            return await memoryCache.GetOrCreateAsync("TOKEN", async entry =>
            {
                var tokenModel = await FetchAccessToken(user, authUrl);
                if (tokenModel != null)
                {
                    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddSeconds(30));
                    memoryCache.Set("TOKEN", tokenModel.Value, options);
                    return tokenModel.Value;
                }
                return null;
            });
        }

        private static async Task<TokenModel?> FetchAccessToken(User user, string authUrl)
        {
            using var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, authUrl);

            var credentials = JsonSerializer.Serialize(user);

            request.Content = new StringContent(credentials, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
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
                    var handler = new JwtSecurityTokenHandler();
                    var validTo = handler.ReadJwtToken(accessToken).ValidTo.ToLocalTime();
                    return new TokenModel { Value = accessToken, ValidTo = validTo };
                case HttpStatusCode.BadRequest:
                    return null;
                default:
                    break;
            }
            return null;
        }
    }
}
