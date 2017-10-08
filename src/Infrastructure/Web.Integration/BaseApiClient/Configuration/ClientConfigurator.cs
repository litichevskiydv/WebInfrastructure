namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using System;
    using System.Net.Http;
    using Flurl.Http;
    using Flurl.Http.Configuration;
    using HttpClientFactories;

    internal class ClientConfigurator : IClientConfigurator
    {
        public ClientConfigurator()
        {
            ClientSettings =
                new ClientFlurlHttpSettings(FlurlHttp.GlobalSettings)
                {
                    HttpClientFactory = new HttpClientFactoryWithDecompressionEnabled()
                };
        }

        public IClientConfigurator WithBaseUrl(string baseUrl)
        {
            if(string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            BaseUrl = baseUrl;
            return this;
        }

        public IClientConfigurator WithTimeout(TimeSpan timeout)
        {
            ClientSettings.Timeout = timeout;
            return this;
        }

        public IClientConfigurator WithSerializer(ISerializer serializer)
        {
            if(serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            ClientSettings.JsonSerializer = serializer;
            return this;
        }

        public IClientConfigurator WithHttpMessageHandler(HttpMessageHandler messageHandler)
        {
            if (messageHandler == null)
                throw new ArgumentNullException(nameof(messageHandler));

            ClientSettings.HttpClientFactory = new HttpClientFactoryWithPredefinedHandler(messageHandler);
            return this;
        }

        public string BaseUrl { get; private set; }
        public ClientFlurlHttpSettings ClientSettings { get; }
    }
}