namespace Skeleton.Web.Tests.Integration.BaseApiClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using JetBrains.Annotations;
    using Web.Integration.BaseApiClient;
    using Xunit;

    public class HttpRequestHeadersExtensionsTests
    {
        #region TestCases

        public class BearerTokenParametersValidationTestCase
        {
            public HttpRequestHeaders Headers { get; set; }

            public string Token { get; set; }
        }

        public class BasicAuthParametersValidationTestCase
        {
            public HttpRequestHeaders Headers { get; set; }

            public string Login { get; set; }

            public string Password { get; set; }
        }

        #endregion

        [UsedImplicitly]
        public static TheoryData<BearerTokenParametersValidationTestCase> BearerTokenParametersValidationTestCases;
        [UsedImplicitly]
        public static TheoryData<BasicAuthParametersValidationTestCase> BasicAuthParametersValidationTestCases;

        static HttpRequestHeadersExtensionsTests()
        {
            BearerTokenParametersValidationTestCases =
                new TheoryData<BearerTokenParametersValidationTestCase>
                {
                    new BearerTokenParametersValidationTestCase {Token = "123"},
                    new BearerTokenParametersValidationTestCase {Headers = new HttpClient().DefaultRequestHeaders},
                    new BearerTokenParametersValidationTestCase {Headers = new HttpClient().DefaultRequestHeaders, Token = "   "}
                };
            BasicAuthParametersValidationTestCases =
                new TheoryData<BasicAuthParametersValidationTestCase>
                {
                    new BasicAuthParametersValidationTestCase
                    {
                        Login = "123",
                        Password = "123"
                    },
                    new BasicAuthParametersValidationTestCase
                    {
                        Headers = new HttpClient().DefaultRequestHeaders,
                        Password = "123"
                    },
                    new BasicAuthParametersValidationTestCase
                    {
                        Headers = new HttpClient().DefaultRequestHeaders,
                        Login = "   ",
                        Password = "123"
                    },
                    new BasicAuthParametersValidationTestCase
                    {
                        Headers = new HttpClient().DefaultRequestHeaders,
                        Login = "123"
                    },
                    new BasicAuthParametersValidationTestCase
                    {
                        Headers = new HttpClient().DefaultRequestHeaders,
                        Login = "123",
                        Password = "   "
                    }
                };
        }

        [Theory]
        [MemberData(nameof(BearerTokenParametersValidationTestCases))]
        public void ShouldValidateBearerTokenParameters(BearerTokenParametersValidationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Headers.WithBearerToken(testCase.Token));
        }

        [Theory]
        [MemberData(nameof(BasicAuthParametersValidationTestCases))]
        public void ShouldValidateBasicAuthParameters(BasicAuthParametersValidationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Headers.WithBasicAuth(testCase.Login, testCase.Password));
        }

        [Fact]
        public void ShouldPlaceSetupAuthorizationHeaderForTokenAuthentication()
        {
            // Given
            const string token = "123";
            var headers = new HttpClient().DefaultRequestHeaders;

            // When
            headers.WithBearerToken(token);

            // Then
            Assert.Equal($"Bearer {token}", headers.Authorization.ToString());
        }

        [Fact]
        public void ShouldPlaceSetupAuthorizationHeaderForBasicAuthentication()
        {
            // Given
            const string login = "123";
            const string password = "123";
            var headers = new HttpClient().DefaultRequestHeaders;

            // When
            headers.WithBasicAuth(login, password);

            // Then
            Assert.Equal(
                $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"))}",
                headers.Authorization.ToString()
            );
        }
    }
}