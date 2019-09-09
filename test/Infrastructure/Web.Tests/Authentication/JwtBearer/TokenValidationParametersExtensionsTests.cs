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

        public class IssuerValidationParametersVerificationTestCase
        {
            public TokenValidationParameters Options { get; set; }

            public string ValidIssuer { get; set; }
        }

        public class IssuersValidationParametersVerificationTestCase
        {
            public TokenValidationParameters Options { get; set; }

            public IEnumerable<string> ValidIssuers { get; set; }
        }

        public class AudienceValidationParametersVerificationTestCase
        {
            public TokenValidationParameters Options { get; set; }

            public  string ValidAudience { get; set; }
        }

        #endregion


        [UsedImplicitly]
        public static readonly TheoryData<IssuerSigningKeyParametersVerificationTestCase> IssuerSigningKeyParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<IssuerSigningKeyResolverParametersVerificationTestCase> IssuerSigningKeyResolverParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<IssuerSigningKeysParametersVerificationTestCase> IssuerSigningKeysParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<IssuerValidationParametersVerificationTestCase> IssuerValidationParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<IssuersValidationParametersVerificationTestCase> IssuersValidationParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<AudienceValidationParametersVerificationTestCase> AudienceValidationParametersVerificationTestCases;
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
            IssuerValidationParametersVerificationTestCases =
                new TheoryData<IssuerValidationParametersVerificationTestCase>
                {
                    new IssuerValidationParametersVerificationTestCase {ValidIssuer = "abc"},
                    new IssuerValidationParametersVerificationTestCase {Options = new TokenValidationParameters()}
                };
            IssuersValidationParametersVerificationTestCases =
                new TheoryData<IssuersValidationParametersVerificationTestCase>
                {
                    new IssuersValidationParametersVerificationTestCase
                    {
                        ValidIssuers = new[] {"abc"}
                    },
                    new IssuersValidationParametersVerificationTestCase
                    {
                        Options = new TokenValidationParameters()
                    },
                    new IssuersValidationParametersVerificationTestCase
                    {
                        Options = new TokenValidationParameters(),
                        ValidIssuers = Enumerable.Empty<string>()
                    }
                };
            AudienceValidationParametersVerificationTestCases =
                new TheoryData<AudienceValidationParametersVerificationTestCase>
                {
                    new AudienceValidationParametersVerificationTestCase {ValidAudience = "abc"},
                    new AudienceValidationParametersVerificationTestCase {Options = new TokenValidationParameters()}
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
        [MemberData(nameof(IssuerValidationParametersVerificationTestCases))]
        public void ShouldValidateIssuerVerificationParameters(IssuerValidationParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithIssuerValidation(testCase.ValidIssuer));
        }

        [Theory]
        [MemberData(nameof(IssuersValidationParametersVerificationTestCases))]
        public void ShouldValidateIssuersVerificationParameters(IssuersValidationParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithIssuerValidation(testCase.ValidIssuers));
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
        [MemberData(nameof(AudienceValidationParametersVerificationTestCases))]
        public void ShouldValidateAudienceVerificationParameters(AudienceValidationParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithAudienceValidation(testCase.ValidAudience));
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