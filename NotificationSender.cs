using Microsoft.Extensions.Caching.Memory;
using RaroNotifications.Exceptions;
using RaroNotifications.Models;
using RaroNotifications.Models.Notifications;
using RaroNotifications.Responses;
using System.Net;
using System.Text;
using System.Text.Json;

namespace RaroNotifications
{
    public class NotificationSender
    {
        private readonly string _authEndpoint = ":3001/api/notification/authentication/sign-in";
        private readonly string _sendNotificationEndpoint = ":3003/api/notification/send";
        private User User { get; set; }
        private readonly IMemoryCache _memoryCache;

        public NotificationSender(IMemoryCache memoryCache, string baseUrl, string username, string password)
        {
            _authEndpoint = baseUrl + _authEndpoint;
            _sendNotificationEndpoint = baseUrl + _sendNotificationEndpoint;
            User = new User { Username = username, Password = password };
            _memoryCache = memoryCache;
        }

        public async Task<NotificationResponse> SendNotification(RequestSendNotificationModel notification)
        {
            string accessToken = await _memoryCache.GetAccessToken(User, _authEndpoint);

            HttpClient client = new();

            var request = new HttpRequestMessage(HttpMethod.Post, _sendNotificationEndpoint);

            var json = JsonSerializer.Serialize(notification);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            try
            {
                var response = await client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        var success = JsonSerializer.Deserialize<NotificationResponse>(content);
                        return success;
                    case HttpStatusCode.BadRequest:
                        var exception = JsonSerializer.Deserialize<NotificationException>(content);
                        throw exception;
                    default:
                        return null;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new NotificationException(ex.StatusCode, ex.Message, DateTime.Now);
            }

        }

        //private static void ValidateReceiver(RequestReceiverSendNotification receiver)
        //{
        //    if (!receiver.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
        //                    BindingFlags.Static | BindingFlags.Instance)
        //        .Any(property => property.CanRead && property.GetValue(receiver, null) != null))
        //    {
        //        throw new Exception();
        //    }
        //}
    }
}