namespace Skeleton.Web.Tests.Authentication.JwtBearer
{
    using System;
    using System.Text;
    using JetBrains.Annotations;
    using Microsoft.IdentityModel.Tokens;
    using Moq;
    using Web.Authentication.JwtBearer;
    using Web.Authentication.JwtBearer.Configuration;
    using Web.Authentication.JwtBearer.Configuration.Issuing;
    using Xunit;

    public class TokensIssuingOptionsExtensionsTests
    {
        #region TestCases

        public class TokenIssueEventHandlerParametersVerificationTestCase
        {
            public TokensIssuingOptions Options { get; set; }

            public ITokenIssueEventHandler EventHandler { get; set; }
        }

        public class GetEndpointParametersVerificationTestCase
        {
            public TokensIssuingOptions Options { get; set; }

            public string Endpoint { get; set; }
        }

        public class SigningKeyParametersVerificationTestCase
        {
            public TokensIssuingOptions Options { get; set; }

            public string SigningAlgorithmName { get; set; }

            public SecurityKey SigningKey { get; set; }
        }

        public class LifetimeParametersVerificationTestCase
        {
            public TokensIssuingOptions Options { get; set; }

            public TimeSpan Lifetime { get; set; }
        }

        #endregion

        [UsedImplicitly]
        public static readonly TheoryData<TokenIssueEventHandlerParametersVerificationTestCase> TokenIssueEventHandlerParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<GetEndpointParametersVerificationTestCase> GetEndpointParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<SigningKeyParametersVerificationTestCase> SigningKeyParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<LifetimeParametersVerificationTestCase> LifetimeParametersVerificationTestCases;

        static TokensIssuingOptionsExtensionsTests()
        {
            GetEndpointParametersVerificationTestCases =
                new TheoryData<GetEndpointParametersVerificationTestCase>
                {
                    new GetEndpointParametersVerificationTestCase {Endpoint = "/api/Account/Token"},
                    new GetEndpointParametersVerificationTestCase {Options = new TokensIssuingOptions()}
                };
            SigningKeyParametersVerificationTestCases =
                new TheoryData<SigningKeyParametersVerificationTestCase>
                {
                    new SigningKeyParametersVerificationTestCase
                    {
                        SigningAlgorithmName = SecurityAlgorithms.HmacSha256,
                        SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))
                    },
                    new SigningKeyParametersVerificationTestCase
                    {
                        Options = new TokensIssuingOptions(),
                        SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))
                    },
                    new SigningKeyParametersVerificationTestCase
                    {
                        Options = new TokensIssuingOptions(),
                        SigningAlgorithmName = SecurityAlgorithms.HmacSha256,
                    }
                };
            LifetimeParametersVerificationTestCases =
                new TheoryData<LifetimeParametersVerificationTestCase>
                {
                    new LifetimeParametersVerificationTestCase {Lifetime = TimeSpan.FromHours(2)}
                };

            TokenIssueEventHandlerParametersVerificationTestCases =
                new TheoryData<TokenIssueEventHandlerParametersVerificationTestCase>
                {
                    new TokenIssueEventHandlerParametersVerificationTestCase {EventHandler = new Mock<ITokenIssueEventHandler>().Object},
                    new TokenIssueEventHandlerParametersVerificationTestCase {Options = new TokensIssuingOptions()}
                };
        }

        [Theory]
        [MemberData(nameof(TokenIssueEventHandlerParametersVerificationTestCases))]
        public void SetTokenIssueEventHandlerFailTest(TokenIssueEventHandlerParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithTokenIssueEventHandler(testCase.EventHandler));
        }

        [Theory]
        [MemberData(nameof(GetEndpointParametersVerificationTestCases))]
        public void WithGetEndpotintShouldValidateInput(GetEndpointParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithGetEndpoint(testCase.Endpoint));
        }

        [Theory]
        [MemberData(nameof(SigningKeyParametersVerificationTestCases))]
        public void WithSigningKeyShouldValidateInput(SigningKeyParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithSigningKey(testCase.SigningAlgorithmName, testCase.SigningKey));
        }

        [Theory]
        [MemberData(nameof(LifetimeParametersVerificationTestCases))]
        public void WithLifetimeShouldValidateInput(LifetimeParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithLifetime(testCase.Lifetime));
        }

        [Fact]
        public void ShouldSetGetEndpoint()
        {
            // Given
            var options = new TokensIssuingOptions();
            const string expectedGetEndpoint = "/api/Account/Token";

            // When
            options.WithGetEndpoint(expectedGetEndpoint);

            // Then
            Assert.Equal(expectedGetEndpoint, options.GetEndpoint);
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
            Assert.Equal(expectedSigningAlgorithmName, options.SigningAlgorithmName);
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