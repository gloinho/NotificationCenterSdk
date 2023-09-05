using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using NotificationCenterSdk.Exceptions;
using NotificationCenterSdk.Models;
using NotificationCenterSdk.Models.Request;
using NotificationCenterSdk.Models.Response;
using NotificationCenterSdk.Tests.Configuration;
using NotificationCenterSdk.Tests.Utils;
using System.Net;
using System.Text.Json;

namespace NotificationCenterSdk.Tests
{
    public class NotificationCenterTest
    {
        private Mock<IHttpClientFactory> _httpClientFactory;
        private Mock<HttpMessageHandler> _mockMessageHandler;
        private Mock<IMemoryCache> _mockMemoryCache;
        private string _baseAddress = "https://localhost:3001/";
        private readonly UserCredentials _user;
        private object _token;
        private readonly Fixture _fixture;

        public NotificationCenterTest()
        {
            _httpClientFactory = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _fixture = FixtureConfig.Get();
            _user = _fixture.Create<UserCredentials>();
            _token = TokenGenerator.GenerateToken(_user);
            _mockMemoryCache.Setup(_ => _.TryGetValue("TOKEN", out _token)).Returns(true);
        }

        [Fact]
        public async void ModelIncorretoDeveLancarExcecao()
        {
            var modelInvalido = _fixture.Create<RequestSendNotification>();
            var exception = _fixture.Build<NotificationException>().With(e => e.StatusCode, HttpStatusCode.BadRequest).Create();
            var jsonResponse = JsonSerializer.Serialize(exception);
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(jsonResponse)
            };

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);
         
            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:3001/")
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationCenter(_mockMemoryCache.Object, _httpClientFactory.Object, _user);
            var result = await Assert.ThrowsAsync<NotificationException>(() => sender.SendNotification(modelInvalido));
            Assert.Equal(exception.StatusCode, result.StatusCode);
        }

        [Fact]
        public async void NotificacaoEnviadaRetornaUmaResponse()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var response = _fixture.Build<NotificationResponse>().With(t => t.Success, true).Create();

            var jsonResponse = JsonSerializer.Serialize(response);
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:3001/")
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationCenter(_mockMemoryCache.Object, _httpClientFactory.Object, _user);

            var result = await sender.SendNotification(model);

            Assert.NotNull(result);
            Assert.Equal(response.Success, result.Success);
        }

        [Fact]
        public async void EnviarNotificacaoETokenValidoRetornaResponse()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var response = _fixture.Build<NotificationResponse>().With(t => t.Success, true).Create();

            var jsonResponse = JsonSerializer.Serialize(response);
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };
     
            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationCenter(_mockMemoryCache.Object, _httpClientFactory.Object, _user);

            var result = await sender.SendNotification(model, _token.ToString());

            Assert.NotNull(result);
            Assert.Equal(response.Success, result.Success);
        }

        [Fact]
        public async void EnviarUmTokenInvalidoLancaExcecao()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var exception = _fixture.Build<AccessTokenException>().With
                (t => t.Message, new List<string>() { "Access Token inválido." }).Create();

            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };
            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationCenter(_mockMemoryCache.Object, _httpClientFactory.Object, _user);
            var result = await Assert.ThrowsAsync<AccessTokenException>(() => sender.SendNotification(model, "tokenInvalido"));
            Assert.Equal(result.Message,exception.Message);
        }

        [Fact]
        public async void EnviarUmTokenExpiradoLancaExcecao()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var exception = _fixture.Build<AccessTokenException>().With
                (t => t.Message, new List<string>() { "Access Token expirado." }).Create();


            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationCenter(_mockMemoryCache.Object, _httpClientFactory.Object, _user);
            var tokenInvalido = TokenGenerator.GenerateExpiredToken(_user);
            var result = await Assert.ThrowsAsync<AccessTokenException>(() => sender.SendNotification(model, tokenInvalido));
            Assert.Equal(exception.Message, result.Message);
        }

        [Fact]
        public async void ApiIndisponivelLancaExcecao()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var exception = _fixture.Build<NotificationException>().With
                    (t => t.StatusCode, HttpStatusCode.InternalServerError).Create();

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException(null, null, statusCode: HttpStatusCode.InternalServerError));

            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationCenter(_mockMemoryCache.Object, _httpClientFactory.Object, _user);
            var result = await Assert.ThrowsAsync<NotificationException>(() => sender.SendNotification(model, _token.ToString()));
            Assert.Equal(exception.StatusCode, result.StatusCode);
        }

        [Fact]
        public async void ErroInternoDaApiLancaExcecao()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var response = _fixture.Build<NotificationException>().With(t => t.StatusCode, HttpStatusCode.InternalServerError).Create();
            var jsonResponse = JsonSerializer.Serialize(response);

            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(jsonResponse)
            };

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationCenter(_mockMemoryCache.Object, _httpClientFactory.Object, _user);
            var result = await Assert.ThrowsAsync<NotificationException>(() => sender.SendNotification(model));
            Assert.Equal(response.StatusCode, result.StatusCode);
        }

        [Fact]
        public async void ApiRetornarStatusCodeDesconhecidoLancaExcecao()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = new StringContent("")
            };

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationCenter(_mockMemoryCache.Object, _httpClientFactory.Object, _user);
            var result = await Assert.ThrowsAsync<NotificationException>(() => sender.SendNotification(model));
            Assert.Null(result.StatusCode);
        }

        [Fact]
        public async void AutenticacaoRetornaUmToken()
        {
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            };
            mockedResponse.Headers.Add("Set-Cookie", $"access_token={_token}");

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);
            var sender = new NotificationCenter(_mockMemoryCache.Object, _httpClientFactory.Object, _user);
            var response = await sender.Authenticate();
            Assert.NotNull(response);
            Assert.Equal(_token.ToString(), response);
        }
    }
}

