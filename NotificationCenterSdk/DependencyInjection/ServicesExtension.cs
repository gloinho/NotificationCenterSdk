using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationCenterSdk.Interfaces;
using NotificationCenterSdk.Models;
using System;

namespace NotificationCenterSdk.DependencyInjection
{
    /// <summary>
    /// Classe responsável por realizar a configuração de DI e as configurações necessárias para o uso da classe <see cref="NotificationCenter"/>.
    /// </summary>
    public static class ServicesExtension
    {
        /// <summary>
        /// Extension Method para configurar o DI e as configurações necessárias para a classe <see cref="NotificationCenter"/>.
        /// Para usar este método, é necessário configurar o appsettings.json corretamente, conforme o exemplo abaixo:
        /// <code>
        /// {
        ///     "NotificationCenter": {
        ///         "AuthBaseUrl": "https://customer-api.example.com",
        ///         "EnginerBaseUrl": "https://enginer-api.example.com",
        ///         "Username": "seu-username",
        ///         "Password": "sua-senha"
        ///     }
        /// }
        /// </code>
        /// </summary>
        /// <param name="configuration">A instância de IConfiguration da aplicação.</param>
        /// <param name="services">A instância de IServiceCollection da aplicação.</param>
        public static void AddNotificationCenter(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetRequiredSection("NotificationCenter");

            services.AddHttpClient("auth", options =>
            {
                options.BaseAddress = new Uri(settings["AuthBaseUrl"]);
            });

            services.AddHttpClient("enginer", options =>
            {
                options.BaseAddress = new Uri(settings["EnginerBaseUrl"]);
            });


            services.AddSingleton(provider =>
            {
                return new UserCredentials
                {
                    Username = settings["Username"],
                    Password = settings["Password"]
                };
            });


            services.AddSingleton<INotificationCenter, NotificationCenter>();
        }
    }
}
    