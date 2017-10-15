namespace Skeleton.Web.Tests.Authentication.JwtBearer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using JetBrains.Annotations;
    using Microsoft.IdentityModel.Tokens;
    using Moq;
    using Web.Authentication.JwtBearer;
    using Web.Authentication.JwtBearer.Configuration;
    using Xunit;

    public class TokensIssuingOptionsExtensionsTests
    {
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithTokenIssueEventHandlerValidationTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithGetEndpotintValidationTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithSigningKeyValidationTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithLifetimeValidationTestsData;

        static TokensIssuingOptionsExtensionsTests()
        {
            WithGetEndpotintValidationTestsData =
                new[]
                {
                    new object[] {null, "/api/Account/Token"},
                    new object[] {new TokensIssuingOptions(), null}
                };
            WithSigningKeyValidationTestsData =
                new[]
                {
                    new object[]
                    {
                        null,
                        SecurityAlgorithms.HmacSha256,
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))
                    },
                    new object[]
                    {
                        new TokensIssuingOptions(),
                        null,
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))
                    },
                    new object[]
                    {
                        new TokensIssuingOptions(),
                        SecurityAlgorithms.HmacSha256,
                        null
                    }
                };
            WithLifetimeValidationTestsData = new[] { new object[] { null, TimeSpan.FromHours(2) } };

            WithTokenIssueEventHandlerValidationTestsData =
                new[]
                {
                    new object[] {null, new Mock<ITokenIssueEventHandler>().Object},
                    new object[] {new TokensIssuingOptions(), null}
                };
        }

        [Theory]
        [MemberData(nameof(WithTokenIssueEventHandlerValidationTestsData))]
        public void SetTokenIssueEventHandlerFailTest(TokensIssuingOptions options, ITokenIssueEventHandler eventHandler)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithTokenIssueEventHandler(eventHandler));
        }

        [Theory]
        [MemberData(nameof(WithGetEndpotintValidationTestsData))]
        public void WithGetEndpotintShouldValidateInput(TokensIssuingOptions options, string endpoint)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithGetEndpotint(endpoint));
        }

        [Theory]
        [MemberData(nameof(WithSigningKeyValidationTestsData))]
        public void WithSigningKeyShouldValidateInput(TokensIssuingOptions options, string signingAlgorithmName, SecurityKey signingKey)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithSigningKey(signingAlgorithmName, signingKey));
        }

        [Theory]
        [MemberData(nameof(WithLifetimeValidationTestsData))]
        public void WithLifetimeShouldValidateInput(TokensIssuingOptions options, TimeSpan lifetime)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithLifetime(lifetime));
        }

        [Fact]
        public void ShouldSetGetEndpoint()
        {
            // Given
            var options = new TokensIssuingOptions();
            const string expectedGetEndpoint = "/api/Account/Token";

            // When
            options.WithGetEndpotint(expectedGetEndpoint);

            // Then
            Assert.Equal((string) expectedGetEndpoint, (string) options.GetEndpotint);
        }

        [Fact]
        public void ShouldSetLifetime()
        {
            // Given
            var options = new TokensIssuingOptions();
            var lifetime = TimeSpan.FromHours(2);

            // When
            options.WithLifetime(lifetime);

            // Then
            Assert.Equal(lifetime, options.Lifetime);
        }

        [Fact]
        public void ShouldSetSigningKey()
        {
            // Given
            var options = new TokensIssuingOptions();
            const string expectedSigningAlgorithmName = SecurityAlgorithms.HmacSha256;
            var expectedSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"));

            // When
            options.WithSigningKey(expectedSigningAlgorithmName, expectedSigningKey);

            // Then
            Assert.Equal((string) expectedSigningAlgorithmName, (string) options.SigningAlgorithmName);
            Assert.Equal(expectedSigningKey, options.SigningKey);
        }

        [Fact]
        public void ShouldSetTokenIssueEventHandlerTest()
        {
            // Given
            var options = new TokensIssuingOptions();
            var tokenIssueEventHandler = new Mock<ITokenIssueEventHandler>().Object;

            // When
            options.WithTokenIssueEventHandler(tokenIssueEventHandler);

            // Then
            Assert.Equal(tokenIssueEventHandler, options.TokenIssueEventHandler);
        }
    }
}