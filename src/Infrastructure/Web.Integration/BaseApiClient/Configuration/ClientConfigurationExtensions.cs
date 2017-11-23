namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Serialization.Abstractions;

    public static class ClientConfigurationExtensions
    {
        private static void ValidateConfigurationArgument(ClientConfiguration configuration)
        {
            if(configuration == null)
                throw new ArgumentNullException(nameof(configuration));
        }

        public static ClientConfiguration WithBaseUrl(this ClientConfiguration configuration, string baseUrl)
        {
            ValidateConfigurationArgument(configuration);
            if(string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            configuration.BaseUrl = baseUrl;
            return configuration;
        }

        public static ClientConfiguration WithTimeout(this ClientConfiguration configuration, TimeSpan timeout)
        {
            ValidateConfigurationArgument(configuration);

            configuration.Timeout = timeout;
            return configuration;
        }

        public static ClientConfiguration WithSerializer(this ClientConfiguration configuration, ISerializer serializer)
        {
            ValidateConfigurationArgument(configuration);
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            configuration.Serializer = serializer;
            return configuration;
        }

        public static ClientConfiguration WithHttpMessageHandlersFactory(
            this ClientConfiguration configuration,
            Func<HttpMessageHandler> messageHandlersFactory = null)
        {
            ValidateConfigurationArgument(configuration);

            configuration.HttpMessageHandlersFactory =
                messageHandlersFactory
                ?? (() => new HttpClientHandler {AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate});
            configuration.DisposeHttpMessageHandlersAfterCall = true;
            return configuration;
        }

        public static ClientConfiguration WithHttpMessageHandler(
            this ClientConfiguration configuration, 
            HttpMessageHandler messageHandler)
        {
            ValidateConfigurationArgument(configuration);
            if (messageHandler == null)
                throw new ArgumentNullException(nameof(messageHandler));

            configuration.HttpMessageHandlersFactory = () => messageHandler;
            configuration.DisposeHttpMessageHandlersAfterCall = false;
            return configuration;
        }
    }
}