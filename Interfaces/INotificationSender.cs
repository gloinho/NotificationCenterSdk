using RaroNotifications.Models.RequestModels;
using RaroNotifications.Responses;

namespace RaroNotifications.Interfaces
{
    public interface INotificationSender
    {
        public Task<NotificationResponse> SendNotification(RequestSendNotification notification);
    }
}
