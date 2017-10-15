namespace Skeleton.Web.Tests.Authentication.JwtBearer
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Moq;
    using Web.Authentication.JwtBearer.Configuration;
    using Xunit;

    public class JwtBearerOptionsExtensionsTests
    {
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithTokenValidationParametersValidationTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithEventsProcessorValidationTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithErrorDetailsValidationTestsData;

        static JwtBearerOptionsExtensionsTests()
        {
            WithTokenValidationParametersValidationTestsData =
                new[]
                {
                    new object[] {null, new Func<TokenValidationParameters, TokenValidationParameters>(x => x)},
                    new object[] {new JwtBearerOptions(), null}
                };
            WithEventsProcessorValidationTestsData =
                new[]
                {
                    new object[] {null, new Mock<JwtBearerEvents>().Object},
                    new object[] {new JwtBearerOptions(), null}
                };
            WithErrorDetailsValidationTestsData = new[] {new object[] {null, true}};
        }

        [Theory]
        [MemberData(nameof(WithTokenValidationParametersValidationTestsData))]
        public void WithTokenValidationParametersValidationShouldValidateInput(JwtBearerOptions options,
            Func<TokenValidationParameters, TokenValidationParameters> parametersBuilder)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithTokenValidationParameters(parametersBuilder));
        }

        [Theory]
        [MemberData(nameof(WithEventsProcessorValidationTestsData))]
        public void WithEventsProcessorShouldValidateInput(JwtBearerOptions options,
            JwtBearerEvents eventsProcessor)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithEventsProcessor(eventsProcessor));
        }

        [Theory]
        [MemberData(nameof(WithErrorDetailsValidationTestsData))]
        public void WithErrorDetailsShouldValidateInput(JwtBearerOptions options,
            bool includeErrorDetails)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithErrorDetails(includeErrorDetails));
        }

        [Fact]
        public void ShouldConfigureTokenValidationParameters()
        {
            // Given
            var options = new JwtBearerOptions();
            const string expectedValidIssuer = "abd";

            // When
            options.WithTokenValidationParameters(x => x.WithIssuerValidation(expectedValidIssuer));

            // Then
            Assert.True(options.TokenValidationParameters.ValidateIssuer);
            Assert.Equal(expectedValidIssuer, options.TokenValidationParameters.ValidIssuer);
        }

        [Fact]
        public void ShouldSetEventsProcessor()
        {
            // Given
            var options = new JwtBearerOptions();
            var expectedEventsProcessor = new Mock<JwtBearerEvents>().Object;

            // When
            options.WithEventsProcessor(expectedEventsProcessor);

            // Then
            Assert.Equal(expectedEventsProcessor, options.Events);
        }

        [Fact]
        public void ShouldAllowErrorDetailsReturning()
        {
            // Given
            var options = new JwtBearerOptions();

            // When
            options.WithErrorDetails();

            // Then
            Assert.True(options.IncludeErrorDetails);
        }

        [Fact]
        public void ShouldDisableErrorDetailsReturning()
        {
            // Given
            var options = new JwtBearerOptions();

            // When
            options
                .WithErrorDetails()
                .WithoutErrorDetails();

            // Then
            Assert.False(options.IncludeErrorDetails);
        }
    }
}