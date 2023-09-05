using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationCenterSdk.Interfaces;
using NotificationCenterSdk.Models;
using System;

namespace NotificationCenterSdk.DependencyInjection
{
    /// <summary>
    /// Classe que realiza o DI e configurações necessárias. <see cref="NotificationCenter"/>
    /// </summary>
    public static class ServicesExtension
    {
        /// <summary>
        /// Extension Method para realizar as configurações necessárias da classe <see cref="NotificationCenter"/>.
        /// É necessário configurar o appsettings.json corretamente de acordo com o exemplo abaixo:
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
        /// <param name="configuration">IConfiguration da aplicação.</param>
        /// <param name="services">ISeviceCollection da aplicação.</param>
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
    