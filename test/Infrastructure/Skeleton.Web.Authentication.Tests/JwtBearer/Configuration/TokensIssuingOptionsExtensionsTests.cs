namespace Skeleton.Web.Authentication.Tests.JwtBearer.Configuration
{
    using System;
    using System.Collections.Generic;
    using Authentication.JwtBearer;
    using Authentication.JwtBearer.Configuration;
    using JetBrains.Annotations;
    using Moq;
    using Xunit;

    public class TokensIssuingOptionsExtensionsTests
    {
        static TokensIssuingOptionsExtensionsTests()
        {
            WithTokenIssueEventHandlerTestsData =
                new[]
                {
                    new object[] {null, new Mock<ITokenIssueEventHandler>().Object},
                    new object[] {new TokensIssuingOptions(), null}
                };
        }

        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithTokenIssueEventHandlerTestsData;

        [Theory]
        [MemberData(nameof(WithTokenIssueEventHandlerTestsData))]
        public void SetTokenIssueEventHandlerFailTest(TokensIssuingOptions options, ITokenIssueEventHandler eventHandler)
        {
            Assert.Throws<ArgumentNullException>(() => options.WithTokenIssueEventHandler(eventHandler));
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