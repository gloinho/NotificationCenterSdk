using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaroNotifications.Interfaces;
using RaroNotifications.Models;

namespace RaroNotifications.DependencyInjection
{
    public static class ServicesExtension
    {
        public static void AddNotificationSender(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetRequiredSection("NotificationSender");

            services.AddHttpClient("customer", options =>
            {
                options.BaseAddress = new Uri(settings["CustomerBaseUrl"]);
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


            services.AddSingleton<INotificationSender, NotificationSender>();
        }
    }
}
    