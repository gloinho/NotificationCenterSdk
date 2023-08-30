using System.Text.Json.Serialization;

namespace RaroNotifications.Models
{
    public class RequestSendNotificationModel
    {
        public RequestSendNotificationModel(RequestReceiverSendNotification receiver, string campaignNameId, HbsTemplateParams? parameters)
        {
            Receiver = receiver;
            CampaignNameId = campaignNameId;
            Parameters = parameters;
        }

        [JsonPropertyName("receiver")]
        public RequestReceiverSendNotification Receiver { get; set; }

        [JsonPropertyName("campaignNameId")]
        public string CampaignNameId { get; set; }
        [JsonPropertyName("parameters")]
        public HbsTemplateParams? Parameters { get; set; }

    }
}
