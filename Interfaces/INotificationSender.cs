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
        /// Realiza requisição para o envio de uma notificação. A autenticação do usuário é realizada de maneira automática e 
        /// o Access Token (JWT) é salvo no Memory Cache da aplicação com a key="TOKEN" com validade igual ao key="exp" do JWT.
        /// </summary>
        /// <param name="notification">A instancia da classe <see cref="RequestSendNotification"/> que representa uma notificação a ser serializada e enviada na requisição</param>
        /// <returns>A instancia da classe <see cref="NotificationResponse"/> representando o retorno da Enginer API.</returns>
        /// <exception cref="NotificationException">
        /// Campos de <paramref name="notification"/> inválidos.
        /// </exception> 
        /// <exception cref="CredentialsException">
        /// Credenciais inválidas.
        /// </exception>  
        /// <exception cref="AccessTokenException">
        /// Access Token inexistente ou inacessível.
        /// </exception>
        public Task<NotificationResponse> SendNotification(RequestSendNotification notification);
        /// <summary>
        /// Realiza requisição para o envio de uma notificação.
        /// </summary>
        /// <param name="notification">A instancia da classe <see cref="RequestSendNotification"/> que representa uma notificação a ser serializada e enviada na requisição</param>
        /// <param name="accessToken">O token JWT para realizar a autenticação no Enginer.</param>
        /// <returns>A instancia da classe <see cref="NotificationResponse"/> representando o retorno da Enginer API.</returns>
        /// <exception cref="NotificationException">
        /// Campos de <paramref name="notification"/> inválidos ou erro de servidor.
        /// </exception>
        /// <exception cref="CredentialsException">
        /// Credenciais inválidas.
        /// </exception>  
        /// <exception cref="AccessTokenException">
        /// Access Token inválido, expirado ou não resgatado.
        /// </exception>
        public Task<NotificationResponse> SendNotification(RequestSendNotification notification, string accessToken);
        public Task<string> Authenticate();
    }
}
