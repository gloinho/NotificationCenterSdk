using System.Text.Json.Serialization;

namespace RaroNotifications.Models.Notifications
{
    /// <summary>
    /// Modelo de requisição enviados para o Enginer API para envio de notificação./>.
    /// </summary>
    public class RequestSendNotification
    {
        /// <summary>
        /// Modelo de requisição serializado enviado para o Enginer API para envio de notificação./>.
        /// </summary>
        /// <param name="receiver">Uma instancia de <see cref="RequestReceiverSendNotification"/> que representa um ou mais receivers da notificação.</param>
        /// <param name="campaignNameId">O campaign_name_id da Campanha relacionada com a notificação.</param>
        /// <param name="parameters">Uma instancia de <see cref="HbsTemplateParams"/> com parametros opcionais.</param>
        public RequestSendNotification(RequestReceiverSendNotification receiver, string campaignNameId, HbsTemplateParams? parameters)
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
