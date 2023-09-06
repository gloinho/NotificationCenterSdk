using System.Text.Json.Serialization;

namespace NotificationCenterSdk.Models.Request
{
    /// <summary>
    /// Modelo de requisição enviados para o Enginer API para envio de notificações./>.
    /// </summary>
    public class RequestSendNotification
    {
        /// <summary>
        /// Inicializa uma nova instância da classe RequestSendNotification.
        /// </summary>
        /// <param name="receiver">Uma instância de <see cref="RequestReceiverSendNotification"/> que representa um ou mais destinatários da notificação.</param>
        /// <param name="campaignNameId">O campaign_name_id da campanha relacionada à notificação.</param>
        /// <param name="parameters">Uma instância de <see cref="HbsTemplateParams"/> com parâmetros opcionais.</param>
        public RequestSendNotification(RequestReceiverSendNotification receiver, string campaignNameId, HbsTemplateParams? parameters)
        {
            Receiver = receiver;
            CampaignNameId = campaignNameId;
            Parameters = parameters;
        }
        /// <summary>
        /// Obtém ou define o(s) destinatário(s) da notificação.
        /// </summary>
        [JsonPropertyName("receiver")]
        public RequestReceiverSendNotification Receiver { get; set; }
        /// <summary>
        /// Obtém ou define o campaign_name_id da campanha relacionada à notificação.
        /// </summary>
        [JsonPropertyName("campaignNameId")]
        public string CampaignNameId { get; set; }
        /// <summary>
        /// Obtém ou define os parâmetros opcionais para a notificação.
        /// </summary>
        [JsonPropertyName("parameters")]
        public HbsTemplateParams Parameters { get; set; }

    }
}
