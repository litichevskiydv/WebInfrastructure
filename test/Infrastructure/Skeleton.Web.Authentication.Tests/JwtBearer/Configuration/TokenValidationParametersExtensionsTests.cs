namespace Skeleton.Web.Authentication.Tests.JwtBearer.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Authentication.JwtBearer.Configuration;
    using JetBrains.Annotations;
    using Microsoft.IdentityModel.Tokens;
    using Xunit;

    public class TokenValidationParametersExtensionsTests
    {
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithIssuerKeyValidationParametersValidationTestsData1;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithIssuerKeyValidationParametersValidationTestsData2;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithIssuerKeyValidationParametersValidationTestsData3;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithIssuerValidationParametersValidationTestsData1;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithIssuerValidationParametersValidationTestsData2;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithAudienceValidationParametersValidationTestsData1;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithAudienceValidationParametersValidationTestsData2;

        static TokenValidationParametersExtensionsTests()
        {
            WithIssuerKeyValidationParametersValidationTestsData1 =
                new[]
                {
                    new object[]
                    {
                        null,
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))
                    },
                    new object[]
                    {
                        new TokenValidationParameters(),
                        null
                    }
                };
            WithIssuerKeyValidationParametersValidationTestsData2 =
                new[]
                {
                    new object[]
                    {
                        null,
                        new IssuerSigningKeyResolver((w, x, y, z) => Enumerable.Empty<SecurityKey>())
                    },
                    new object[]
                    {
                        new TokenValidationParameters(),
                        null
                    }
                };
            WithIssuerKeyValidationParametersValidationTestsData3 =
                new[]
                {
                    new object[]
                    {
                        null,
                        new[] {new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))}
                    },
                    new object[]
                    {
                        new TokenValidationParameters(),
                        null
                    },
                    new object[]
                    {
                        new TokenValidationParameters(),
                        Enumerable.Empty<SecurityKey>()
                    }
                };
            WithIssuerValidationParametersValidationTestsData1 =
                new[]
                {
                    new object[] {null, "abc"},
                    new object[] {new TokenValidationParameters(), null}
                };
            WithIssuerValidationParametersValidationTestsData2 =
                new[]
                {
                    new object[] {null, new[] {"abc"}},
                    new object[] {new TokenValidationParameters(), null},
                    new object[] {new TokenValidationParameters(), Enumerable.Empty<string>()}
                };
            WithAudienceValidationParametersValidationTestsData1 =
                new[]
                {
                    new object[] {null, "abc"},
                    new object[] {new TokenValidationParameters(), null}
                };
            WithAudienceValidationParametersValidationTestsData2 =
                new[]
                {
                    new object[] {null, new[] {"abc"}},
                    new object[] {new TokenValidationParameters(), null},
                    new object[] {new TokenValidationParameters(), Enumerable.Empty<string>()}
                };
        }

        [Theory]
        [MemberData(nameof(WithIssuerKeyValidationParametersValidationTestsData1))]
        public void WithIssuerKeyValidationShouldValidateInput1(TokenValidationParameters options, SecurityKey securityKey)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithIssuerKeyValidation(securityKey));
        }

        [Theory]
        [MemberData(nameof(WithIssuerKeyValidationParametersValidationTestsData2))]
        public void WithIssuerKeyValidationShouldValidateInput2(TokenValidationParameters options, IssuerSigningKeyResolver securityKeyResolver)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithIssuerKeyValidation(securityKeyResolver));
        }

        [Theory]
        [MemberData(nameof(WithIssuerKeyValidationParametersValidationTestsData3))]
        public void WithIssuerKeyValidationShouldValidateInput3(TokenValidationParameters options, IEnumerable<SecurityKey> securityKeys)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithIssuerKeyValidation(securityKeys));
        }

        [Fact]
        public void WithoutIssuerKeyValidationShouldValidateInput()
        {
            // Given
            TokenValidationParameters options = null;

            // When, Then
            Assert.Throws<ArgumentNullException>(() => options.WithoutIssuerKeyValidation());
        }

        [Fact]
        public void WithLifetimeValidationShouldValidateInput()
        {
            // Given
            TokenValidationParameters options = null;

            // When, Then
            Assert.Throws<ArgumentNullException>(() => options.WithLifetimeValidation());
        }

        [Fact]
        public void WithoutLifetimeValidationShouldValidateInput()
        {
            // Given
            TokenValidationParameters options = null;

            // When, Then
            Assert.Throws<ArgumentNullException>(() => options.WithoutLifetimeValidation());
        }

        [Theory]
        [MemberData(nameof(WithIssuerValidationParametersValidationTestsData1))]
        public void WithIssuerValidationShouldValidateInput1(TokenValidationParameters options, string validIssuer)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithIssuerValidation(validIssuer));
        }

        [Theory]
        [MemberData(nameof(WithIssuerValidationParametersValidationTestsData2))]
        public void WithIssuerValidationShouldValidateInput2(TokenValidationParameters options, IEnumerable<string> validIssuers)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithIssuerValidation(validIssuers));
        }

        [Fact]
        public void WithoutIssuerValidationShouldValidateInput()
        {
            // Given
            TokenValidationParameters options = null;

            // When, Then
            Assert.Throws<ArgumentNullException>(() => options.WithoutIssuerValidation());
        }

        [Theory]
        [MemberData(nameof(WithAudienceValidationParametersValidationTestsData1))]
        public void WithAudienceValidationShouldValidateInput1(TokenValidationParameters options, string validAudience)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithAudienceValidation(validAudience));
        }

        [Theory]
        [MemberData(nameof(WithAudienceValidationParametersValidationTestsData2))]
        public void WithAudienceValidationShouldValidateInput2(TokenValidationParameters options, IEnumerable<string> validAudiences)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithAudienceValidation(validAudiences));
        }

        [Fact]
        public void WithoutAudienceValidationShouldValidateInput()
        {
            // Given
            TokenValidationParameters options = null;

            // When, Then
            Assert.Throws<ArgumentNullException>(() => options.WithoutAudienceValidation());
        }
    }
}