using RaroNotifications.Exceptions;
using RaroNotifications.Models.Request;
using RaroNotifications.Models.Response;

namespace RaroNotifications.Interfaces
{
    /// <summary>
    /// Interface do <see cref="NotificationSender"/>.
    /// </summary>
    public interface INotificationSender
    {
        /// <summary>
        /// Realiza requisição para o envio de uma notificação.
        /// </summary>
        /// <param name="notification">A instancia da classe <see cref="RequestSendNotification"/> que representa uma notificação a ser serializada e enviada na requisição</param>
        /// <returns>A instancia da classe <see cref="NotificationResponse"/> representando o retorno da Enginer API.</returns>
        /// <exception cref="NotificationException">
        /// Campos de <paramref name="notification"/> inválidos.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// Não foi possivel realizar a requisição.
        /// </exception>   
        /// <exception cref="CredentialsException">
        /// Credenciais inválidas.
        /// </exception>  
        /// <exception cref="AccessTokenException">
        /// Access Token inexistente ou inacessível.
        /// </exception>
        public Task<NotificationResponse> SendNotification(RequestSendNotification notification);
    }
}
