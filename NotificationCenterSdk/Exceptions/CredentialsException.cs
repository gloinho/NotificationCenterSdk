using System;
using System.Net;

namespace NotificationCenterSdk.Exceptions
{
    /// <summary>
    /// Exceção que representa erros relacionados a credenciais inválidas.
    /// </summary>
    public class CredentialsException : NotificationException
    {
        /// <summary>
        /// Inicializa uma nova instância da classe CredentialsException com as informações fornecidas.
        /// </summary>
        /// <param name="statusCode">O código de status HTTP associado à exceção.</param>
        /// <param name="message">A mensagem de erro da exceção.</param>
        /// <param name="timeStamp">A data e hora em que a exceção ocorreu.</param>
        /// <param name="detail">Detalhes adicionais relacionados à exceção.</param>
        /// <param name="path">O caminho ou rota relacionado à exceção.</param>
        public CredentialsException(HttpStatusCode? statusCode, string message, DateTime timeStamp, string? detail = null, string? path = null)
            : base(statusCode, message, timeStamp, detail, path)
        {
        }
    }
}
