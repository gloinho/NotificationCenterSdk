using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationCenterSdk.Tests.Utils
{
    public static class FakeClientFactoryGenerator
    {
        public static IHttpClientFactory Get(HttpMessageHandler handler)
        {
            Mock<IHttpClientFactory> _httpClientFactory = new();

            var fakeClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://fake.adddress/")
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(fakeClient);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(fakeClient);

            return _httpClientFactory.Object;
        }
    }
}
