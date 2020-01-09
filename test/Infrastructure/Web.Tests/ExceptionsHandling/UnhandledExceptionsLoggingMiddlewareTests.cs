namespace Skeleton.Web.Tests.ExceptionsHandling
{
    using System;
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
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Testing.Extensions;
    using Web.ExceptionsHandling;
    using Web.Serialization.Jil.Configuration;
    using Xunit;

    public class UnhandledExceptionsLoggingMiddlewareTests
    {
        private class TestHttpResponseStreamWriterFactory : IHttpResponseStreamWriterFactory
        {
            private const int DefaultBufferSize = 16 * 1024;

            public TextWriter CreateWriter(Stream stream, Encoding encoding)
            {
                return new HttpResponseStreamWriter(stream, encoding, DefaultBufferSize);
            }
        }

        private readonly Mock<ILogger> _mockLogger;

        [UsedImplicitly]
        public static TheoryData<string> EnvironmentsWhereExceptionDetailAvailable;

        static UnhandledExceptionsLoggingMiddlewareTests()
        {
            EnvironmentsWhereExceptionDetailAvailable =
                new TheoryData<string>
                {
                    Environments.Development,
                    Environments.Staging
                };
        }

        public UnhandledExceptionsLoggingMiddlewareTests()
        {
            _mockLogger = MockLoggerExtensions.CreateMockLogger();
        }

        private IHost ConfigureHost(string environmentName) =>
            new HostBuilder()
                .UseEnvironment(environmentName)
                .ConfigureWebHost(
                    webBuilder => webBuilder
                        .UseMockLogger(_mockLogger)
                        .UseTestServer()
                        .Configure(
                            app =>
                                app
                                    .UseUnhandledExceptionsLoggingMiddleware()
                                    .Run(context => throw new InvalidOperationException("Wrong state"))
                        )
                )
                .Start();


        [Theory]
        [MemberData(nameof(EnvironmentsWhereExceptionDetailAvailable))]
        public async Task ShouldSendExceptionMessageToClientInDevelopmentAndStagingEnvironmets(string environmentName)
        {
            // Given
            using var host = ConfigureHost(environmentName);

            // When, Then
            var response = await host.GetTestClient().GetAsync("/");
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Wrong state", content);

            _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();
        }

        [Fact]
        public async Task ShouldNotReturnExceptionMessageToClientInProductionEnvironment()
        {
            // Given
            using var host = ConfigureHost(Environments.Production);

            // When, Then
            var response = await host.GetTestClient().GetAsync("/");
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            Assert.Empty(content);

            _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();
        }

        [Fact]
        public async Task ShouldLogWarningAndReturnBadRequestOnTaskCancellation()
        {
            // Given
            using var host = new HostBuilder()
                .ConfigureWebHost(
                    webBuilder => webBuilder
                        .UseMockLogger(_mockLogger)
                        .UseTestServer()
                        .Configure(
                            app =>
                                app
                                    .UseUnhandledExceptionsLoggingMiddleware()
                                    .Run(
                                        context =>
                                        {
                                            var source = new CancellationTokenSource();
                                            source.CancelAfter(TimeSpan.FromMilliseconds(100));

                                            return Task.Delay(TimeSpan.FromSeconds(5), source.Token);
                                        })
                        ))
                .Start();

            // When, Then
            var response = await host.GetTestClient().GetAsync("/");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Empty(content);

            _mockLogger.VerifyWarningWasLogged();
        }

        [Fact]
        public async Task ShouldSendExceptionToClientViaConfiguredFormatters()
        {
            // Given
            using var host = new HostBuilder()
                .UseEnvironment(Environments.Development)
                .ConfigureWebHost(
                    webBuilder => webBuilder
                        .UseMockLogger(_mockLogger)
                        .ConfigureServices(
                            services => services
                                .AddSingleton<IHttpResponseStreamWriterFactory, TestHttpResponseStreamWriterFactory>()
                                .AddMvc(x => { })
                                .WithJsonFormattersBasedOnJil(OptionsExtensions.Default)
                        )
                        .UseTestServer()
                        .Configure(
                            app =>
                                app
                                    .UseUnhandledExceptionsLoggingMiddleware()
                                    .Run(context => throw new InvalidOperationException("Wrong state"))
                        )
                )
                .Start();

            // When, Then
            var response = await host
                .GetTestServer()
                .CreateRequest("/")
                .AddHeader("accept", "application/json")
                .SendAsync("GET");
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Wrong state", content);

            _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();
        }
    }
}