using AutoFixture;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using NotificationCenterSdk.Managers;
using NotificationCenterSdk.Models;
using NotificationCenterSdk.Tests.Configuration;
using NotificationCenterSdk.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NotificationCenterSdk.Tests.ManagerTests
{
    public class AccessTokenCacheManagerTest
    {
        private Mock<HttpMessageHandler> _mockMessageHandler;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly string _url = "http://localhost:3001/api/notification/authentication/sign-in";
        private readonly Fixture _fixture;
        private readonly HttpClient _client;
        private readonly string _token;
        private readonly UserCredentials _user;

        public AccessTokenCacheManagerTest()
        {
            _fixture = FixtureConfig.Get();
            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _user = _fixture.Create<UserCredentials>();
            _token = TokenGenerator.GenerateToken(_user);
            

            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            };
            mockedResponse.Headers.Add("Set-Cookie", $"access_token={_token}");

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            _client = new HttpClient(_mockMessageHandler.Object);
        }

        [Fact]
        public async void SeNaoTiverUmTokenCriaUmNovoTokenNaMemoria()
        {
            var entry = Mock.Of<ICacheEntry>();
            entry.Value = _token;

            object callBack;
            
            _mockMemoryCache.Setup(_ => _.TryGetValue("TOKEN", out callBack)).Returns(false);
            _mockMemoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(entry);

            var managerResponse = await AccessTokenCacheManager.RetrieveOrCreateAccessToken(_mockMemoryCache.Object, _user, _url, _client);
            Assert.NotNull(managerResponse);
            Assert.Equal(entry.Value, managerResponse);
        }

        [Fact]
        public async void SeJaTiverUmTokenNaoRealizaOFetch()
        {
            object callback = _token;
            _mockMemoryCache.Setup(_ => _.TryGetValue("TOKEN", out callback)).Returns(true);

            var managerResponse = await AccessTokenCacheManager.RetrieveOrCreateAccessToken(_mockMemoryCache.Object, _user, _url, _client);

            Assert.NotNull(managerResponse);
            Assert.Equal(_token, managerResponse);
        }
    }
}
