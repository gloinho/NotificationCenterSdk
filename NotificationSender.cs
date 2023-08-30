using Microsoft.Extensions.Caching.Memory;
using raro_notifications.Models;

namespace raro_notifications
{
    public class NotificationSender
    {
        private string _authUrl { get; set; }
        private string _sendNotificationUrl { get; set; }
        private User User { get; set; }
        private readonly IMemoryCache _memoryCache;

        public NotificationSender(IMemoryCache memoryCache, string authUrl, string sendNotificationUrl, string username, string password)
        {
            _authUrl = authUrl;
            _sendNotificationUrl = sendNotificationUrl;
            User = new User { Username = username, Password = password };
            _memoryCache = memoryCache;
        }

        public async Task<bool> SendNotification(string notification)
        {
            var accessToken = await _memoryCache.GetAccessToken(User, _authUrl) ?? throw new Exception("deu ruim");

            HttpClient client = new();

            var request = new HttpRequestMessage(HttpMethod.Post, _sendNotificationUrl);

            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            var response = await client.SendAsync(request);

            return response.StatusCode == System.Net.HttpStatusCode.Created;
        }
    }
}