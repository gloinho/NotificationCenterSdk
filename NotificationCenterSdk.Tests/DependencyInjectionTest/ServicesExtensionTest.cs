using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationCenterSdk.Models;
using NotificationCenterSdk.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationCenterSdk.Tests.Utils;
using Moq;

namespace NotificationCenterSdk.Tests.DependencyInjectionTest
{
    public class ServicesExtensionTest
    {
        private Mock<IMemoryCache> _mockMemoryCache;
        public ServicesExtensionTest()
        {
            object token;
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockMemoryCache.Setup(_ => _.TryGetValue("TOKEN", out token)).Returns(true);
        }
        [Fact]
        public void MetodoDeExtensaoConfiguraCorretamenteAsDependencias()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                new KeyValuePair<string, string>("NotificationCenter:AuthBaseUrl", "https://example.com/auth"),
                new KeyValuePair<string, string>("NotificationCenter:EnginerBaseUrl", "https://example.com/enginer"),
                new KeyValuePair<string, string>("NotificationCenter:Username", "yourUsername"),
                new KeyValuePair<string, string>("NotificationCenter:Password", "yourPassword")
                })
                .Build();

            var services = new ServiceCollection();
            
            services.AddNotificationCenter(configuration);

            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var authHttpClient = httpClientFactory.CreateClient("auth");
            var enginerHttpClient = httpClientFactory.CreateClient("enginer");
            var userCredentials = serviceProvider.GetRequiredService<UserCredentials>();

            Assert.NotNull(authHttpClient);
            Assert.NotNull(enginerHttpClient);
            Assert.Equal(new Uri("https://example.com/auth"), authHttpClient.BaseAddress);
            Assert.Equal(new Uri("https://example.com/enginer"), enginerHttpClient.BaseAddress);
            Assert.Equal("yourUsername", userCredentials.Username);
            Assert.Equal("yourPassword", userCredentials.Password);

            var center = new NotificationCenter(_mockMemoryCache.Object, httpClientFactory, userCredentials);
            Assert.NotNull(center);
        }
    }
}
