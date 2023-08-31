namespace RaroNotifications.Models.Notifications
{
    /// <summary>
    /// Modelo de parametros opcionais enviados para o Enginer API para envio de notificação./>
    /// </summary>
    public class HbsTemplateParams
    {
        /// <summary>
        /// Inicializa uma nova instancia de <see cref="HbsTemplateParams"/>
        /// </summary>
        /// <param name="parameters">Dicionário contendo os parametros opcionais</param>
        public HbsTemplateParams(Dictionary<string,string> parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Parametros opcionais./>
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }
    }
}