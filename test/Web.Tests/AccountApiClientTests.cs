namespace Web.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using Client;
    using Skeleton.Web.Integration.Exceptions;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    public class AccountApiClientTests : BaseApiClientTests<BaseApiTestsFixture<TestsStartup>, ApiClient>
    {
        public AccountApiClientTests(BaseApiTestsFixture<TestsStartup> fixture) : base(fixture)
        {
        }

        [Fact]
        public void ShouldGetUserInfo()
        {
            // Given
            const string login = "lhp@lhp.com";
            const string password = "1234";

            // When
            ApiClient
                .Login(login, password)
                .UserInfo();

            // Then
            Fixture.MockLogger.VerifyNoErrors();
            Assert.Equal($"{ClaimTypes.Email}:{login}", ((IEnumerable<string>) ApiClient.CurrentState).First());
        }

        [Fact]
        public void ShouldThrowUnauthorizedExceptionWhileAccessingUserInfoWithoutToken()
        {
            Assert.Throws<UnauthorizedException>(() => ApiClient.UserInfo());
        }

        [Fact]
        public void ShouldThrowNotFoundExceptionWhileWhenLoginIsIncorrect()
        {
            Assert.Throws<NotFoundException>(() => ApiClient.Login("lhp1@lhp.com", "1234"));
        }

        [Fact]
        public void ShouldReciveForbiddenWhenPasswordIsIncorrect()
        {
            var exception = Assert.Throws<ApiException>(() => ApiClient.Login("lhp@lhp.com", "12345"));
            Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
        }
    }
}