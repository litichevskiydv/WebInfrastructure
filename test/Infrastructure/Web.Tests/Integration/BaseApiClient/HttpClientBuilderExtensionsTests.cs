namespace Skeleton.Web.Tests.Integration.BaseApiClient
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Moq;
    using Web.Integration.BaseApiClient;
    using Web.Integration.BaseApiClient.Configuration;
    using Web.Serialization.Jil.Serializer;
    using Xunit;

    public class HttpClientBuilderExtensionsTests
    {
        #region TestCases

        public class ClientConfigurationValidationTestCase
        {
            public IHttpClientBuilder HttpClientBuilder { get; set; }

            public IConfiguration Config { get; set; }

            public Action<OptionsBuilder<FakeClientOptions>> OptionsConfigurator { get; set; }
        }
        

        #endregion

        [UsedImplicitly]
        public class FakeClientOptions : BaseClientOptions
        {
        }

        [UsedImplicitly]
        public class FakeClient : BaseClient
        {
            public FakeClient(HttpClient httpClient, IOptions<FakeClientOptions> options) : base(httpClient, options.Value)
            {
            }

            public int Get()
            {
                return Get<int>("/test");
            }
        }

        private class RequestsCapturingHandler : DelegatingHandler
        {
            public HttpRequestMessage CapturedRequest { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                CapturedRequest = request;
                return base.SendAsync(request, cancellationToken);
            }
        }

        [UsedImplicitly]
        public static TheoryData<ClientConfigurationValidationTestCase> ClientConfigurationValidationTestCases;
        [UsedImplicitly]
        public static IEnumerable<object[]> UseDefaultPrimaryMessageHandlerValidationTestsData;

        static HttpClientBuilderExtensionsTests()
        {
            ClientConfigurationValidationTestCases
                = new TheoryData<ClientConfigurationValidationTestCase>
                  {
                      new ClientConfigurationValidationTestCase
                      {
                          Config = new Mock<IConfiguration>().Object,
                          OptionsConfigurator = x => { }
                      },
                      new ClientConfigurationValidationTestCase
                      {
                          HttpClientBuilder = new Mock<IHttpClientBuilder>().Object,
                          OptionsConfigurator = x => { }
                      },
                      new ClientConfigurationValidationTestCase
                      {
                          HttpClientBuilder = new Mock<IHttpClientBuilder>().Object,
                          Config = new Mock<IConfiguration>().Object
                      }
                  };
            UseDefaultPrimaryMessageHandlerValidationTestsData
                = new[]
                  {
                      new object[]
                      {
                          null,
                          new Func<HttpClientHandler, HttpClientHandler>(x => x)
                      },
                      new object[]
                      {
                          new Mock<IHttpClientBuilder>().Object,
                          null
                      }
                  };
        }

        [Theory]
        [MemberData(nameof(ClientConfigurationValidationTestCases))]
        public void ShouldValidateClientConfiguration(ClientConfigurationValidationTestCase test)
        {
            Assert.Throws<ArgumentNullException>(() => test.HttpClientBuilder.ConfigureClient(test.Config, test.OptionsConfigurator));
        }

        [Theory]
        [MemberData(nameof(UseDefaultPrimaryMessageHandlerValidationTestsData))]
        public void UseDefaultPrimaryMessageHandlerShouldValidateParameters(
            IHttpClientBuilder httpClientBuilder,
            Func<HttpClientHandler, HttpClientHandler> handlerConfigurator)
        {
            Assert.Throws<ArgumentNullException>(() => httpClientBuilder.UseDefaultPrimaryMessageHandler(handlerConfigurator));
        }

        [Fact]
        public void ShouldConfigureDefaultPrimaryMessageHandler()
        {
            // Given
            var requstsCapturer = new RequestsCapturingHandler();
            var serviceProvider = new ServiceCollection()
                .Configure<FakeClientOptions>(
                    x =>
                    {
                        x.BaseUrl = "http://mmm.co";
                        x.Timeout = TimeSpan.FromSeconds(3);
                        x.Serializer = JilSerializer.Default;
                    }
                )
                .AddClient<FakeClient>()
                .UseDefaultPrimaryMessageHandler(x => x.WithAutomaticDecompression())
                .AddHttpMessageHandler(() => requstsCapturer)
                .Services.BuildServiceProvider();

            // When
            var client = serviceProvider.GetService<FakeClient>();
            try
            {
                client.Get();
            }
            catch (Exception)
            {
            }

            // Then
            Assert.Contains(
                requstsCapturer.CapturedRequest.Headers.AcceptEncoding,
                x => string.Equals(x.Value, "gzip", StringComparison.InvariantCultureIgnoreCase)
            );
            Assert.Contains(
                requstsCapturer.CapturedRequest.Headers.AcceptEncoding,
                x => string.Equals(x.Value, "deflate", StringComparison.InvariantCultureIgnoreCase)
            );
        }
    }
}