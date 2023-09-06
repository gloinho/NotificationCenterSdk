using NotificationCenterSdk.Exceptions;
using NotificationCenterSdk.Models.Request;
using NotificationCenterSdk.Models.Response;
using System.Threading.Tasks;

namespace NotificationCenterSdk.Interfaces
{
    /// <summary>
    /// Interface para comunicação com o serviço de notificação.
    /// </summary>
    public interface INotificationCenter
    {
        /// <summary>
        /// Envia uma notificação para o serviço de notificação. A autenticação do usuário é realizada automaticamente,
        /// e o Access Token (JWT) é armazenado em cache com a chave "TOKEN", com base no "exp" do JWT.
        /// </summary>
        /// <param name="notification">Instância da classe <see cref="RequestSendNotification"/> que representa a notificação a ser serializada e enviada na requisição.</param>
        /// <returns>Instância da classe <see cref="NotificationResponse"/> que representa a resposta do serviço de notificação.</returns>
        /// <exception cref="NotificationException">Lançada quando os campos de <paramref name="notification"/> são inválidos ou ocorre um erro no servidor.</exception>
        /// <exception cref="CredentialsException">Lançada quando as credenciais são inválidas.</exception>
        /// <exception cref="AccessTokenException">Lançada se o Access Token for inexistente ou inacessível. </exception>
        public Task<NotificationResponse> SendNotification(RequestSendNotification notification);

        /// <summary>
        /// Envia uma notificação para o serviço de notificação usando um token JWT de autenticação.
        /// </summary>
        /// <param name="notification">Instância da classe <see cref="RequestSendNotification"/> que representa a notificação a ser serializada e enviada na requisição.</param>
        /// <param name="accessToken">Token JWT usado para autenticação no serviço de notificação.</param>
        /// <returns>Instância da classe <see cref="NotificationResponse"/> que representa a resposta do serviço de notificação.</returns>
        /// <exception cref="NotificationException">Lançada quando os campos de <paramref name="notification"/> são inválidos ou ocorre um erro no servidor.</exception>
        /// <exception cref="CredentialsException">Lançada quando as credenciais são inválidas.</exception>
        /// <exception cref="AccessTokenException">Lançada quando o Access Token é inválido, expirou ou não foi recuperado.</exception>
        public Task<NotificationResponse> SendNotification(RequestSendNotification notification, string accessToken);
        /// <summary>
        /// Realiza a autenticação do usuário e retorna o Access Token (JWT) necessário para o envio de notificações.
        /// </summary>
        /// <returns>Access Token (JWT) necessário para o envio de notificações.</returns>
        /// <exception cref="CredentialsException">Lançada se as credenciais fornecidas forem inválidas durante a autenticação.</exception>
        /// <exception cref="AccessTokenException">Lançada se o Access Token não puder ser recuperado durante a autenticação.</exception>
        public Task<string> Authenticate();
    }
}
