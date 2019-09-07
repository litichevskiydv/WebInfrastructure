namespace Skeleton.Web.Tests.Authentication.JwtBearer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using JetBrains.Annotations;
    using Microsoft.IdentityModel.Tokens;
    using Web.Authentication.JwtBearer.Configuration;
    using Xunit;

    public class TokenValidationParametersExtensionsTests
    {
        #region TestCases

        public class IssuerSigningKeyParametersVerificationTestCase
        {
            public TokenValidationParameters Options { get; set; }

            public SecurityKey SecurityKey { get; set; }
        }

        public class IssuerSigningKeyResolverParametersVerificationTestCase
        {
            public TokenValidationParameters Options { get; set; }

            public IssuerSigningKeyResolver SecurityKeyResolver { get; set; }
        }

        public class IssuerSigningKeysParametersVerificationTestCase
        {
            public TokenValidationParameters Options { get; set; }

            public IEnumerable<SecurityKey> SecurityKeys { get; set; }
        }

        #endregion


        [UsedImplicitly]
        public static readonly TheoryData<IssuerSigningKeyParametersVerificationTestCase> IssuerSigningKeyParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<IssuerSigningKeyResolverParametersVerificationTestCase> IssuerSigningKeyResolverParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<IssuerSigningKeysParametersVerificationTestCase> IssuerSigningKeysParametersVerificationTestCases;
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
            IssuerSigningKeyParametersVerificationTestCases =
                new TheoryData<IssuerSigningKeyParametersVerificationTestCase>
                {
                    new IssuerSigningKeyParametersVerificationTestCase
                    {
                        SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))
                    },
                    new IssuerSigningKeyParametersVerificationTestCase
                    {
                        Options = new TokenValidationParameters()
                    }
                };
            IssuerSigningKeyResolverParametersVerificationTestCases =
                new TheoryData<IssuerSigningKeyResolverParametersVerificationTestCase>
                {
                    new IssuerSigningKeyResolverParametersVerificationTestCase
                    {
                        SecurityKeyResolver = (w, x, y, z) => Enumerable.Empty<SecurityKey>()
                    },
                    new IssuerSigningKeyResolverParametersVerificationTestCase
                    {
                        Options = new TokenValidationParameters()
                    }
                };
            IssuerSigningKeysParametersVerificationTestCases =
                new TheoryData<IssuerSigningKeysParametersVerificationTestCase>
                {
                    new IssuerSigningKeysParametersVerificationTestCase
                    {
                        SecurityKeys = new[] {new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))}
                    },
                    new IssuerSigningKeysParametersVerificationTestCase
                    {
                        Options = new TokenValidationParameters()
                    },
                    new IssuerSigningKeysParametersVerificationTestCase
                    {
                        Options = new TokenValidationParameters(),
                        SecurityKeys = Enumerable.Empty<SecurityKey>()
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
        [MemberData(nameof(IssuerSigningKeyParametersVerificationTestCases))]
        public void ShouldValidateIssuerSigningKeyParameters(IssuerSigningKeyParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithIssuerKeyValidation(testCase.SecurityKey));
        }

        [Theory]
        [MemberData(nameof(IssuerSigningKeyResolverParametersVerificationTestCases))]
        public void ShouldValidateIssuerSigningKeyResolverParameters(IssuerSigningKeyResolverParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithIssuerKeyValidation(testCase.SecurityKeyResolver));
        }

        [Theory]
        [MemberData(nameof(IssuerSigningKeysParametersVerificationTestCases))]
        public void ShouldValidateIssuerSigningKeysParameters(IssuerSigningKeysParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithIssuerKeyValidation(testCase.SecurityKeys));
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

        [Fact]
        public void ShouldSetSecurityKey()
        {
            // Given
            var options = new TokenValidationParameters();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"));
            var validator = new IssuerSigningKeyValidator((x, y, z) => true);

            // When
            options.WithIssuerKeyValidation(key, validator);

            // Then
            Assert.True(options.ValidateIssuerSigningKey);
            Assert.Equal(key, options.IssuerSigningKey);
            Assert.Equal(validator, options.IssuerSigningKeyValidator);
        }

        [Fact]
        public void ShouldSetSecurityKeyResolver()
        {
            // Given
            var options = new TokenValidationParameters();
            var resolver = new IssuerSigningKeyResolver(
                (w, x, y, z) =>
                    new[] {new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))});
            var validator = new IssuerSigningKeyValidator((x, y, z) => true);

            // When
            options.WithIssuerKeyValidation(resolver, validator);

            // Then
            Assert.True(options.ValidateIssuerSigningKey);
            Assert.Equal(resolver, options.IssuerSigningKeyResolver);
            Assert.Equal(validator, options.IssuerSigningKeyValidator);
        }

        [Fact]
        public void ShouldSetSecurityKeys()
        {
            // Given
            var options = new TokenValidationParameters();
            var keys = new[] {new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ"))};
            var validator = new IssuerSigningKeyValidator((x, y, z) => true);

            // When
            options.WithIssuerKeyValidation(keys, validator);

            // Then
            Assert.True(options.ValidateIssuerSigningKey);
            Assert.Equal(keys, options.IssuerSigningKeys);
            Assert.Equal(validator, options.IssuerSigningKeyValidator);
        }

        [Fact]
        public void ShouldDeactivateIssuerKeyValidation()
        {
            // Given
            var options = new TokenValidationParameters();

            // When
            options
                .WithIssuerKeyValidation(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ")))
                .WithoutIssuerKeyValidation();

            // Then
            Assert.False(options.ValidateIssuerSigningKey);
        }

        [Fact]
        public void ShouldActivateLifetimeValidation()
        {
            // Given
            var options = new TokenValidationParameters();
            var validator = new LifetimeValidator((w, x, y, z) => true);

            // When
            options.WithLifetimeValidation(validator);

            // Then
            Assert.True(options.ValidateLifetime);
            Assert.Equal(validator, options.LifetimeValidator);
        }

        [Fact]
        public void ShouldDeactivateLifetimeValidation()
        {
            // Given
            var options = new TokenValidationParameters();

            // When
            options
                .WithLifetimeValidation()
                .WithoutLifetimeValidation();

            // Then
            Assert.False(options.ValidateLifetime);
        }

        [Fact]
        public void ShouldSetValidIssuer()
        {
            // Given
            var options = new TokenValidationParameters();
            const string issuer = "https://issuer.example.com";

            // When
            options.WithIssuerValidation(issuer);

            // Then
            Assert.True(options.ValidateIssuer);
            Assert.Equal(issuer, options.ValidIssuer);
        }

        [Fact]
        public void ShouldSetValidIssuers()
        {
            // Given
            var options = new TokenValidationParameters();
            var issuers = new[] {"https://issuer.example.com"};

            // When
            options.WithIssuerValidation(issuers);

            // Then
            Assert.True(options.ValidateIssuer);
            Assert.Equal(issuers, options.ValidIssuers);
        }

        [Fact]
        public void ShouldDeactivateIssuerValidation()
        {
            // Given
            var options = new TokenValidationParameters();

            // When
            options
                .WithIssuerValidation("https://issuer.example.com")
                .WithoutIssuerValidation();

            // Then
            Assert.False(options.ValidateIssuer);
        }

        [Fact]
        public void ShouldSetValidAudience()
        {
            // Given
            var options = new TokenValidationParameters();
            const string audience = "https://yourapplication.example.com";

            // When
            options.WithAudienceValidation(audience);

            // Then
            Assert.True(options.ValidateAudience);
            Assert.Equal(audience, options.ValidAudience);
        }

        [Fact]
        public void ShouldSetValidAudiences()
        {
            // Given
            var options = new TokenValidationParameters();
            var audiences = new[] {"https://yourapplication.example.com"};

            // When
            options.WithAudienceValidation(audiences);

            // Then
            Assert.True(options.ValidateAudience);
            Assert.Equal(audiences, options.ValidAudiences);
        }

        [Fact]
        public void ShouldDeactivateAudienceValidation()
        {
            // Given
            var options = new TokenValidationParameters();

            // When
            options
                .WithAudienceValidation("https://yourapplication.example.com")
                .WithoutAudienceValidation();

            // Then
            Assert.False(options.ValidateAudience);
        }
    }
}