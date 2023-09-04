using AutoFixture;
using Moq;
using Moq.Protected;
using RaroNotifications.Exceptions;
using RaroNotifications.Models;
using RaroNotifications.Models.Request;
using RaroNotifications.Models.Response;
using RaroNotifications.Tests.Configuration;
using RaroNotifications.Tests.Utils;
using System.Net;
using System.Text.Json;

namespace RaroNotifications.Tests
{
    public class NotificationSenderTest
    {
        private Mock<IHttpClientFactory> _httpClientFactory;
        private Mock<HttpMessageHandler> _mockMessageHandler;
        private string _baseAddress = "https://localhost:3001/";
        private readonly UserCredentials _user;
        private readonly Fixture _fixture;

        public NotificationSenderTest()
        {
            _httpClientFactory = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _fixture = FixtureConfig.Get(); 
            _user = _fixture.Create<UserCredentials>();
        }

        [Fact]
        public async void ModelIncorretoDeveLancarErro()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
            };

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object);

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationSender(new MemoryCacheFake(), _httpClientFactory.Object, _user);
            var result = Assert.ThrowsAsync<NotificationException>(() => sender.SendNotification(model));

        }

        [Fact]
        public async void NotificacaoEnviadaRetornaUmaResponse()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var token = TokenGenerator.GenerateToken(_user);
            var response = _fixture.Create<NotificationResponse>();

            var jsonResponse = JsonSerializer.Serialize(response);
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(jsonResponse)
            };

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress=new Uri("http://localhost:3001/")
            };

            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationSender(new MemoryCacheFake(token), _httpClientFactory.Object, _user);

            var result = await sender.SendNotification(model);

            Assert.NotNull(result);
            Assert.Equal(response.Id, result.Id);
        }

        [Fact]
        public async void EnviarNotificacaoETokenValidoRetornaResponse()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var token = TokenGenerator.GenerateToken(_user);
            var response = _fixture.Create<NotificationResponse>();

            var jsonResponse = JsonSerializer.Serialize(response);
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
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

            var sender = new NotificationSender(new MemoryCacheFake(token), _httpClientFactory.Object, _user);

            var result = await sender.SendNotification(model, token);

            Assert.NotNull(result);
            Assert.Equal(response.Id,result.Id);
        }

        [Fact]
        public async void EnviarUmTokenInvalidoRetornaErro()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var response = _fixture.Create<NotificationResponse>();
            var jsonResponse = JsonSerializer.Serialize(response);
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
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

            var sender = new NotificationSender(new MemoryCacheFake(), _httpClientFactory.Object, _user);
            var result = Assert.ThrowsAsync<AccessTokenException>(() => sender.SendNotification(model,"tokenInvalido"));
        }

        [Fact]
        public async void EnviarUmTokenExpiradoRetornaErro()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var response = _fixture.Create<NotificationResponse>();
            var jsonResponse = JsonSerializer.Serialize(response);
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
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

            var sender = new NotificationSender(new MemoryCacheFake(), _httpClientFactory.Object, _user);
            var tokenInvalido = TokenGenerator.GenerateExpiredToken(_user);
            var result = Assert.ThrowsAsync<AccessTokenException>(() => sender.SendNotification(model, tokenInvalido));

        }

        [Fact]
        public async void ApiIndisponivelRetornaUmErro()
        {
            var model = _fixture.Create<RequestSendNotification>();
            var response = _fixture.Create<NotificationResponse>();
            var jsonResponse = JsonSerializer.Serialize(response);

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException(null,null,statusCode:HttpStatusCode.InternalServerError));

            var client = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };
            _httpClientFactory.Setup(_ => _.CreateClient("enginer")).Returns(client);
            _httpClientFactory.Setup(_ => _.CreateClient("auth")).Returns(client);

            var sender = new NotificationSender(new MemoryCacheFake(), _httpClientFactory.Object, _user);
            var token = TokenGenerator.GenerateToken(_user);
            var result = await Assert.ThrowsAsync<NotificationException>(() => sender.SendNotification(model, token));
        }
    }
}

