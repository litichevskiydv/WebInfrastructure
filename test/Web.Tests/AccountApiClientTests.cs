namespace Web.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Client;
    using Skeleton.Web.Integration.BaseApiClient.Exceptions;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    [Collection(nameof(ApiTestsCollection))]
    public class AccountApiClientTests : BaseApiClientTests<Startup>
    {
        private readonly ApiClient _defaultClient;

        public AccountApiClientTests(BaseApiTestsFixture<Startup> fixture): base(fixture)
        {
            _defaultClient = CreateClient<ApiClient, ApiClientOptions>();
        }

        [Fact]
        public void ShouldGetUserInfo()
        {
            // Given
            const string login = "lhp@lhp.com";
            const string password = "1234";

            // When
            _defaultClient
                .Login(login, password)
                .UserInfo();

            // Then
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
            Assert.Equal($"{ClaimTypes.Email}:{login}", ((IEnumerable<string>)_defaultClient.CurrentState).First());
        }

        [Fact]
        public void ShouldThrowUnauthorizedExceptionWhileAccessingUserInfoWithoutToken()
        {
            Assert.Throws<UnauthorizedException>(() => _defaultClient.UserInfo());
        }

        [Fact]
        public void ShouldReturnBadRequestWhenLoginNotProvided()
        {
            Assert.Throws<BadRequestException>(() => _defaultClient.Login(null, "1234"));
        }

        [Fact]
        public void ShouldReturnBadRequestWhenLoginIsIncorrect()
        {
            Assert.Throws<BadRequestException>(() => _defaultClient.Login("lhp1@lhp.com", "1234"));
        }

        [Fact]
        public void ShouldReturnBadRequestWhenPasswordIsIncorrect()
        {
            Assert.Throws<BadRequestException>(() => _defaultClient.Login("lhp@lhp.com", "12345"));
        }

        [Fact]
        public void ShouldReturnBadRequestWhenPasswordNotProvided()
        {
            Assert.Throws<BadRequestException>(() => _defaultClient.Login("lhp@lhp.com", null));
        }
    }
}