using RaroNotifications.Models;

namespace RaroNotifications.Responses
{
    public class NotificationSuccess : NotificationResult
    {
        public string? Id { get; set; }
        public SendNotificationProvider Providers { get; set; }
        public List<SendNotificationProvider>? ProviderFallOver { get; set; }
        public SendNotificationTemplate? Template { get; set; }
    }
}
