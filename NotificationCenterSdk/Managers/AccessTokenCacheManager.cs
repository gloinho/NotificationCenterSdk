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
            bool tokenExists = memoryCache.TryGetValue("TOKEN", out object token);
            if (tokenExists)
            {
                return token.ToString();
            }
            var tokenModel = await UserAuthenticationManager.FetchAccessToken(userCredentials, authUrl, httpClient);

            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(tokenModel.ValidTo);
  
            return memoryCache.Set("TOKEN", tokenModel.Value, options);
        }
    }
}
