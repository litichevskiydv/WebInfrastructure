namespace Skeleton.Web.Tests.Integration.BaseApiClient
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Jil;
    using Newtonsoft.Json;
    using Web.Integration.BaseApiClient.Configuration;
    using Web.Integration.BaseApiClient.Serializers;
    using Xunit;

    public class ClientConfiguratorTests
    {
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithBaseUrlValidationTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithJsonNetSerializerValidationTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> WithJilSerializerValidationTestsData;

        static ClientConfiguratorTests()
        {
            WithBaseUrlValidationTestsData =
                new[]
                {
                    new object[] {null},
                    new object[] {string.Empty}
                };
            WithJsonNetSerializerValidationTestsData =
                new[]
                {
                    new object[] {null, new JsonSerializerSettings()},
                    new object[] {new ClientConfigurator(), null}
                };
            WithJilSerializerValidationTestsData =
                new[]
                {
                    new object[] {null, Options.Default},
                    new object[] {new ClientConfigurator(), null}
                };
        }

        [Theory]
        [MemberData(nameof(WithBaseUrlValidationTestsData))]
        public void ShouldNotValidateBaseUrl(string baseUrl)
        {
            // Given
            var configurator = new ClientConfigurator();

            // When, Then
            Assert.Throws<ArgumentNullException>(() => configurator.WithBaseUrl(baseUrl));
        }

        [Fact]
        public void ShouldSetBasUrl()
        {
            // Given
            const string baseUrl = "http://localhost";
            var configurator = new ClientConfigurator();

            // When
            configurator.WithBaseUrl(baseUrl);

            // Then
            Assert.Equal(baseUrl, configurator.BaseUrl);
        }

        [Fact]
        public void ShouldSetTimeout()
        {
            // Given
            var timeout = TimeSpan.FromMinutes(1);
            var configurator = new ClientConfigurator();

            // When
            configurator.WithTimeout(timeout);

            // Then
            Assert.Equal(timeout, configurator.ClientSettings.Timeout);
        }

        [Fact]
        public void ShouldNotValidateSerializer()
        {
            // Given
            var configurator = new ClientConfigurator();

            // When, Then
            Assert.Throws<ArgumentNullException>(() => configurator.WithSerializer(null));
        }

        [Theory]
        [MemberData(nameof(WithJsonNetSerializerValidationTestsData))]
        public void WithJsonNetSerializerShouldNotValidateArguments(
            IClientConfigurator configurator,
            JsonSerializerSettings serializerSettings)
        {
            Assert.Throws<ArgumentNullException>(() => configurator.WithJsonNetSerializer(serializerSettings));
        }

        [Theory]
        [MemberData(nameof(WithJilSerializerValidationTestsData))]
        public void WithWithJilSerializerShouldNotValidateArguments(
            IClientConfigurator configurator,
            Options serializerSettings)
        {
            Assert.Throws<ArgumentNullException>(() => configurator.WithJilSerializer(serializerSettings));
        }

        [Fact]
        public void ShouldSetSerializer()
        {
            // Given
            var serializer = new JilJsonSerializer(Options.Default);
            var configurator = new ClientConfigurator();

            // When
            configurator.WithSerializer(serializer);

            // Then
            Assert.Equal(serializer, configurator.ClientSettings.JsonSerializer);
        }

        [Fact]
        public void ShouldNotValidateHttpMessageHandler()
        {
            // Given
            var configurator = new ClientConfigurator();

            // When, Then
            Assert.Throws<ArgumentNullException>(() => configurator.WithHttpMessageHandler(null));
        }
    }
}