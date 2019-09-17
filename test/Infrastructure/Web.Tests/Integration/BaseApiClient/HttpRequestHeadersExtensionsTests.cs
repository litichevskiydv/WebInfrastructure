namespace Skeleton.Web.Tests.Integration.BaseApiClient
{
    using System;
    using System.Collections.Generic;
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

        #endregion

        [UsedImplicitly]
        public static TheoryData<BearerTokenParametersValidationTestCase> BearerTokenParametersValidationTestCases;
        [UsedImplicitly]
        public static IEnumerable<object[]> WithBasicAuthParametersValidationTestsData;

        static HttpRequestHeadersExtensionsTests()
        {
            BearerTokenParametersValidationTestCases =
                new TheoryData<BearerTokenParametersValidationTestCase>
                {
                    new BearerTokenParametersValidationTestCase {Token = "123"},
                    new BearerTokenParametersValidationTestCase {Headers = new HttpClient().DefaultRequestHeaders},
                    new BearerTokenParametersValidationTestCase {Headers = new HttpClient().DefaultRequestHeaders, Token = "   "}
                };
            WithBasicAuthParametersValidationTestsData =
                new[]
                {
                    new object[] {null, "123", "123"},
                    new object[] {new HttpClient().DefaultRequestHeaders, null, "123"},
                    new object[] {new HttpClient().DefaultRequestHeaders, "   ", "123"},
                    new object[] {new HttpClient().DefaultRequestHeaders, "123", null},
                    new object[] {new HttpClient().DefaultRequestHeaders, "123", "   "}
                };
        }

        [Theory]
        [MemberData(nameof(BearerTokenParametersValidationTestCases))]
        public void ShouldValidateBearerTokenParameters(BearerTokenParametersValidationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Headers.WithBearerToken(testCase.Token));
        }

        [Theory]
        [MemberData(nameof(WithBasicAuthParametersValidationTestsData))]
        public void WithBasicAuthShouldNotValidateParameters(HttpRequestHeaders headers, string login, string password)
        {
            Assert.Throws<ArgumentNullException>(() => headers.WithBasicAuth(login, password));
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