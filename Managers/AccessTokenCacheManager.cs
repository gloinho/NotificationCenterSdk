﻿using Microsoft.Extensions.Caching.Memory;
using RaroNotifications.Models;

namespace RaroNotifications.Manager
{
    internal static class AccessTokenCacheManager
    {
        internal static async Task<string> RetrieveOrCreateAccessToken(this IMemoryCache memoryCache, 
            UserCredentials userCredentials, string authUrl, HttpClient httpClient)
        {
            var token = memoryCache.Get<string>("TOKEN");
            if (token != null)
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
