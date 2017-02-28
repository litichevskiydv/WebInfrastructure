namespace Skeleton.Web.Authentication.Tests.JwtBearer.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Authentication.JwtBearer;
    using Authentication.JwtBearer.Configuration;
    using JetBrains.Annotations;
    using Microsoft.IdentityModel.Tokens;
    using Xunit;

    public class TokensIssuingOptionsExtensionsTests
    {
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
            WithLifetimeValidationTestsData = new[] {new object[] {null, TimeSpan.FromHours(2)}};
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
        public void WithLifetimeShouldValidateInput(TokensIssuingOptions options, TimeSpan? lifetime)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithLifetime(lifetime));
        }
    }
}