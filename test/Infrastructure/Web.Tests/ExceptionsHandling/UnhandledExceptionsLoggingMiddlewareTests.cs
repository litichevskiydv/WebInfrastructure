namespace Skeleton.Web.Tests.ExceptionsHandling
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Testing.Extensions;
    using Web.ExceptionsHandling;
    using Web.Serialization.Jil.Configuration;
    using Xunit;

    public class UnhandledExceptionsLoggingMiddlewareTests
    {
        public class TestHttpResponseStreamWriterFactory : IHttpResponseStreamWriterFactory
        {
            public const int DefaultBufferSize = 16 * 1024;

            public TextWriter CreateWriter(Stream stream, Encoding encoding)
            {
                return new HttpResponseStreamWriter(stream, encoding, DefaultBufferSize);
            }
        }

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

        [Fact]
        public async Task ShouldLogWarningAndReturnBadRequestOnTaskCancellation()
        {
            // Given
            var hostBuilder = new WebHostBuilder()
                .UseMockLogger(_mockLogger)
                .Configure(app =>
                           {
                               app.UseUnhandledExceptionsLoggingMiddleware();
                               app.Run(context =>
                                       {
                                           var source = new CancellationTokenSource();
                                           source.CancelAfter(TimeSpan.FromMilliseconds(100));

                                           return Task.Delay(TimeSpan.FromSeconds(5), source.Token);
                                       });
                           });

            // When, Then
            using (var server = new TestServer(hostBuilder))
            {
                var response = await server.CreateRequest("/").SendAsync("GET");
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                Assert.Empty(content);

                _mockLogger.VerifyWarningWasLogged();
            }
        }

        [Fact]
        public async Task ShouldSendExceptionToClientViaConfiguredFormatters()
        {
            // Given
            var hostBuilder = new WebHostBuilder()
                .UseEnvironment(EnvironmentName.Development)
                .UseMockLogger(_mockLogger)
                .ConfigureServices(services => services
                                       .AddSingleton<IHttpResponseStreamWriterFactory, TestHttpResponseStreamWriterFactory>()
                                       .AddMvc(x => { })
                                       .WithJsonFormattersBasedOnJil(OptionsExtensions.Default)
                )
                .Configure(app =>
                           {
                               app.UseUnhandledExceptionsLoggingMiddleware();
                               app.Run(context => throw new InvalidOperationException("Wrong state"));
                           }
                );

            // When, Then
            using (var server = new TestServer(hostBuilder))
            {
                var response = await server.CreateRequest("/").AddHeader("accept", "application/json").SendAsync("GET");
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                Assert.Contains("Wrong state", content);

                _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();
            }
        }
    }
}