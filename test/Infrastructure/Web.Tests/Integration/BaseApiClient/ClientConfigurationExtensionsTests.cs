namespace Skeleton.Web.Tests.Integration.BaseApiClient
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using JetBrains.Annotations;
    using Jil;
    using Web.Integration.BaseApiClient.Configuration;
    using Web.Serialization.Jil.Serializer;
    using Xunit;

    public class ClientConfigurationExtensionsTests
    {
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithBaseUrlValidationTestsData;

        static ClientConfigurationExtensionsTests()
        {
            WithBaseUrlValidationTestsData =
                new[]
                {
                    new object[] {new ClientConfiguration(), null},
                    new object[] {new ClientConfiguration(), string.Empty},
                    new object[] {null, "http://127.0.0.1"}
                };
        }

        [Theory]
        [MemberData(nameof(WithBaseUrlValidationTestsData))]
        public void ShouldNotValidateBaseUrl(ClientConfiguration configuration, string baseUrl)
        {
            // When, Then
            Assert.Throws<ArgumentNullException>(() => configuration.WithBaseUrl(baseUrl));
        }

        [Fact]
        public void ShouldSetBasUrl()
        {
            // Given
            const string baseUrl = "http://localhost";
            var configuration = new ClientConfiguration();

            // When
            configuration.WithBaseUrl(baseUrl);

            // Then
            Assert.Equal(baseUrl, configuration.BaseUrl);
        }

        [Fact]
        public void ShouldSetTimeout()
        {
            // Given
            var timeout = TimeSpan.FromMinutes(1);
            var configuration = new ClientConfiguration();

            // When
            configuration.WithTimeout(timeout);

            // Then
            Assert.Equal(timeout, configuration.Timeout);
        }

        [Fact]
        public void ShouldNotValidateSerializer()
        {
            // Given
            var configuration = new ClientConfiguration();

            // When, Then
            Assert.Throws<ArgumentNullException>(() => configuration.WithSerializer(null));
        }

        [Fact]
        public void ShouldSetSerializer()
        {
            // Given
            var serializer = new JilSerializer(Options.Default);
            var configuration = new ClientConfiguration();

            // When
            configuration.WithSerializer(serializer);

            // Then
            Assert.Equal(serializer, configuration.Serializer);
        }

        [Fact]
        public void ShouldNotValidateHttpMessageHandler()
        {
            // Given
            var configuration = new ClientConfiguration();

            // When, Then
            Assert.Throws<ArgumentNullException>(() => configuration.WithHttpMessageHandler(null));
        }

        [Fact]
        public void ShouldSetHttpMessageHandler()
        {
            // Given
            var configuration = new ClientConfiguration();
            var messageHandler = new HttpClientHandler();

            // When
            configuration.WithHttpMessageHandler(messageHandler);

            // Then
            Assert.Equal(messageHandler, configuration.HttpMessageHandlersFactory());
            Assert.False(configuration.DisposeHttpMessageHandlersAfterCall);
        }

        [Fact]
        public void ShouldSetDefaultHttpMessageHandlersFactory()
        {
            // Given
            var configuration = new ClientConfiguration();

            // When
            configuration.WithHttpMessageHandlersFactory();

            // Then
            Assert.NotNull(configuration.HttpMessageHandlersFactory);
            Assert.True(configuration.DisposeHttpMessageHandlersAfterCall);
        }
    }
}