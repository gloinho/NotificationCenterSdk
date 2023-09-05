using Microsoft.Extensions.Caching.Memory;
using NotificationCenterSdk.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotificationCenterSdk.Managers
{
    internal static class AccessTokenCacheManager
    {
        internal static async Task<string> RetrieveOrCreateAccessToken(this IMemoryCache memoryCache,
            UserCredentials userCredentials, string authUrl, HttpClient httpClient)
        {
            bool tokenExists = memoryCache.TryGetValue("TOKEN", out string token);
            if (tokenExists)
            {
                return token;
            }
            var tokenModel = await UserAuthenticationManager.FetchAccessToken(userCredentials, authUrl, httpClient);

            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(tokenModel.ValidTo);
            memoryCache.Set("TOKEN", tokenModel.Value, options);

            return tokenModel.Value;
        }
    }
}
