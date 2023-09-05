using AutoFixture;
using Moq;
using Moq.Protected;
using NotificationCenterSdk.Exceptions;
using NotificationCenterSdk.Managers;
using NotificationCenterSdk.Models;
using NotificationCenterSdk.Tests.Configuration;
using NotificationCenterSdk.Tests.Utils;
using System.Net;
using System.Text.Json;

namespace NotificationCenterSdk.Tests.ManagerTests
{
    public class UserAuthenticationManagerTest
    {
        private Mock<HttpMessageHandler> _mockMessageHandler;
        private readonly string _url = "http://localhost:3001/api/notification/authentication/sign-in";
        private readonly Fixture _fixture;

        public UserAuthenticationManagerTest()
        {
            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _fixture = FixtureConfig.Get();
        }

        [Fact]
        public async void AutenticacaoDeveRetornarUmAccessTokenModelPreenchido()
        {
            var user = _fixture.Create<UserCredentials>();
            var token = TokenGenerator.GenerateToken(user);

            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            };
            mockedResponse.Headers.Add("Set-Cookie", $"access_token={token}");

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object);

            var result = await UserAuthenticationManager.FetchAccessToken(user, _url, client);
            Assert.NotNull(result);
            Assert.Equal(token, result.Value);
        }

        [Fact]
        public async void LoginInvalidoDeveLancarErro()
        {
            var user = _fixture.Create<UserCredentials>();
            var exception = _fixture.Build<CredentialsException>().With(e => e.StatusCode, HttpStatusCode.BadRequest).Create();
            var jsonResponse = JsonSerializer.Serialize(exception);
            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(jsonResponse)
            };
            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object);

            var result = await Assert.ThrowsAsync<CredentialsException>(() => UserAuthenticationManager.FetchAccessToken(user, _url, client));
            Assert.Equal(exception.StatusCode, result.StatusCode);
        }

        [Fact]
        public async void CookieInexistenteDeveLancarErro()
        {
            var user = _fixture.Create<UserCredentials>();
            var token = TokenGenerator.GenerateToken(user);

            var mockedResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            };

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var client = new HttpClient(_mockMessageHandler.Object);

            var result = Assert.ThrowsAsync<AccessTokenException>(() => UserAuthenticationManager.FetchAccessToken(user, _url, client));
        }

        [Fact]
        public async void ErroDeServidorDeveLancarErro()
        {
            var user = _fixture.Create<UserCredentials>();
            _mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException(null, null, HttpStatusCode.BadRequest));

            var client = new HttpClient(_mockMessageHandler.Object);

            var result = Assert.ThrowsAsync<AccessTokenException>(() => UserAuthenticationManager.FetchAccessToken(user, _url, client));
            var code = result.Result.StatusCode;

            Assert.Equal(HttpStatusCode.InternalServerError, code);
        }
    }
}
