using System.Text.Json.Serialization;

namespace RaroNotifications.Responses
{
    public class NotificationException : Exception
    {
        public NotificationException()
        {
        }

        public List<string> Message { get; set; }
        public string Detail { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Path { get; set; }
    }
}
