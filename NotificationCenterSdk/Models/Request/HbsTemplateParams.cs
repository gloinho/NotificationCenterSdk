using System.Collections.Generic;

namespace NotificationCenterSdk.Models.Request
{
    /// <summary>
    /// Modelo de parâmetros opcionais enviados para a Enginer API no contexto do envio de notificações.
    /// </summary>
    public class HbsTemplateParams
    {
        /// <summary>
        /// Inicializa uma nova instância da classe HbsTemplateParams.
        /// </summary>
        /// <param name="parameters">Um dicionário contendo os parâmetros opcionais a serem enviados.</param>
        public HbsTemplateParams(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Obtém ou define os parâmetros opcionais a serem enviados para a Enginer API.
        /// </summary>
        public Dictionary<string, string>? Parameters { get; set; }
    }
}