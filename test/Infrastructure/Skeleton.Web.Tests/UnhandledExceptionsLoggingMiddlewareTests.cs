namespace Skeleton.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using ExceptionsHandling;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Testing.Extensions;
    using Xunit;

    public class UnhandledExceptionsLoggingMiddlewareTests
    {
        private readonly Mock<ILogger> _mockLogger;

        [UsedImplicitly]
        public static IEnumerable<object[]> EnvironmentsWhereExceptionDetailAvailable;

        static UnhandledExceptionsLoggingMiddlewareTests()
        {
            EnvironmentsWhereExceptionDetailAvailable =
                new[]
                {
                    new object[] {EnvironmentName.Development},
                    new object[] {EnvironmentName.Staging}
                };
        }

        public UnhandledExceptionsLoggingMiddlewareTests()
        {
            _mockLogger = MockLoggerExtensions.CreateMockLogger();
        }

        private IWebHostBuilder ConfigureHost(string environmentName)
        {
            return new WebHostBuilder()
                .UseEnvironment(environmentName)
                .UseMockLogger(_mockLogger)
                .Configure(app =>
                           {
                               app.UseUnhandledExceptionsLoggingMiddleware();
                               app.Run(context =>
                                       {
                                           throw new InvalidOperationException("Wrong state");
                                       });
                           });
        }

        [Theory]
        [MemberData(nameof(EnvironmentsWhereExceptionDetailAvailable))]
        public async Task ShouldSendExceptionMessageToClientInDevelopmentAndStagingEnvironmets(string environmentName)
        {
            // Given
            var hostBuilder = ConfigureHost(environmentName);

            // When, Then
            using (var server = new TestServer(hostBuilder))
            {
                var response = await server.CreateRequest("/").SendAsync("GET");
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                Assert.Contains("Wrong state", content);

                _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();
            }
        }

        [Fact]
        public async Task ShouldNotReturnExceprionMessageToClientInProductionEnvironment()
        {
            // Given
            var hostBuilder = ConfigureHost(EnvironmentName.Production);

            // When, Then
            using (var server = new TestServer(hostBuilder))
            {
                var response = await server.CreateRequest("/").SendAsync("GET");
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                Assert.Empty(content);

                _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();
            }
        }
    }
}