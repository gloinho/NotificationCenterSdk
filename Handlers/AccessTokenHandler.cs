﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using RaroNotifications.Exceptions;
using RaroNotifications.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace RaroNotifications.Handlers
{
    internal static class AccessTokenHandler
    {
        internal static async Task<string> GetAccessToken(this IMemoryCache memoryCache, User user, string authUrl)
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

        private static async Task<AccessTokenModel> FetchAccessToken(User user, string authUrl)
        {
            using var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, authUrl);

            var credentials = JsonSerializer.Serialize(user);

            request.Content = new StringContent(credentials, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.SendAsync(request);
                string accessToken = string.Empty;

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
                throw httpException;
            }
        }
    }
}
