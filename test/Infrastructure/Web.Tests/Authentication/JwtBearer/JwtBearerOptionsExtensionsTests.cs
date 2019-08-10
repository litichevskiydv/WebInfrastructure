namespace Skeleton.Web.Tests.Authentication.JwtBearer
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Moq;
    using Web.Authentication.JwtBearer.Configuration;
    using Xunit;

    public class JwtBearerOptionsExtensionsTests
    {
        #region TestCases

        public class TokenValidationParametersVerificationTestCase
        {
            public JwtBearerOptions Options { get; set; }

            public Func<TokenValidationParameters, TokenValidationParameters> ParametersBuilder { get; set; }
        }

        public class EventsProcessorParametersVerificationTestCase
        {
            public JwtBearerOptions Options { get; set; }

            public JwtBearerEvents EventsProcessor { get; set; }
        }

        public class ErrorsProcessingParametersVerificationTestCase
        {
            public JwtBearerOptions Options { get; set; }

            public bool IncludeErrorDetails { get; set; }
        }

        #endregion

        [UsedImplicitly]
        public static readonly TheoryData<TokenValidationParametersVerificationTestCase> TokenValidationParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<EventsProcessorParametersVerificationTestCase> EventsProcessorParametersVerificationTestCases;
        [UsedImplicitly]
        public static readonly TheoryData<ErrorsProcessingParametersVerificationTestCase> ErrorsProcessingParametersVerificationTestCases;

        static JwtBearerOptionsExtensionsTests()
        {
            TokenValidationParametersVerificationTestCases =
                new TheoryData<TokenValidationParametersVerificationTestCase>
                {
                    new TokenValidationParametersVerificationTestCase {ParametersBuilder = x => x},
                    new TokenValidationParametersVerificationTestCase {Options = new JwtBearerOptions()}
                };
            EventsProcessorParametersVerificationTestCases =
                new TheoryData<EventsProcessorParametersVerificationTestCase>
                {
                    new EventsProcessorParametersVerificationTestCase {EventsProcessor = new Mock<JwtBearerEvents>().Object},
                    new EventsProcessorParametersVerificationTestCase {Options = new JwtBearerOptions()}
                };
            ErrorsProcessingParametersVerificationTestCases =
                new TheoryData<ErrorsProcessingParametersVerificationTestCase>
                {
                    new ErrorsProcessingParametersVerificationTestCase {IncludeErrorDetails = true}
                };
        }

        [Theory]
        [MemberData(nameof(TokenValidationParametersVerificationTestCases))]
        public void WithTokenValidationParametersValidationShouldValidateInput(TokenValidationParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithTokenValidationParameters(testCase.ParametersBuilder));
        }

        [Theory]
        [MemberData(nameof(EventsProcessorParametersVerificationTestCases))]
        public void WithEventsProcessorShouldValidateInput(EventsProcessorParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithEventsProcessor(testCase.EventsProcessor));
        }

        [Theory]
        [MemberData(nameof(ErrorsProcessingParametersVerificationTestCases))]
        public void WithErrorDetailsShouldValidateInput(ErrorsProcessingParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Options.WithErrorDetails(testCase.IncludeErrorDetails));
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