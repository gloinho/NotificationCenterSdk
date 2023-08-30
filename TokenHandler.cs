using Microsoft.Extensions.Caching.Memory;
using raro_notifications.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace raro_notifications
{
    public static class TokenHandler
    {
        public static async Task<string> GetAccessToken(this IMemoryCache memoryCache, User user, string authUrl)
        {
            await memoryCache.GetOrCreateAsync("TOKEN", async entry =>
            {
                var tokenModel = await FetchAccessToken(user, authUrl);
                var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(tokenModel.ValidTo);

                memoryCache.Set("TOKEN", tokenModel.Value, options);

                return tokenModel.Value;
            });

            return memoryCache.Get("TOKEN").ToString();

        }

        private static async Task<TokenModel> FetchAccessToken(User user, string authUrl)
        {
            HttpClient client = new();

            var request = new HttpRequestMessage(HttpMethod.Post, authUrl);

            var credentials = JsonSerializer.Serialize(user);

            request.Content = new StringContent(credentials);

            var response = await client.SendAsync(request);

            var jwt = await response.Content.ReadAsStringAsync();

            var handler = new JwtSecurityTokenHandler();

            var validTo = handler.ReadJwtToken(jwt).ValidTo;

            return new TokenModel { Value = jwt, ValidTo = validTo };
        }
    }
}
