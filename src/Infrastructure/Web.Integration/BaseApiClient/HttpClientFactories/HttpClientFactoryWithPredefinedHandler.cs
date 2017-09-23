namespace Skeleton.Web.Integration.BaseApiClient.HttpClientFactories
{
    using System;
    using System.Net.Http;
    using Flurl.Http.Configuration;
    using Flurl.Http.Testing;

    internal class HttpClientFactoryWithPredefinedHandler : IHttpClientFactory
    {
        private readonly HttpMessageHandler _messageHandler;

        public HttpClientFactoryWithPredefinedHandler(HttpMessageHandler messageHandler)
        {
            if (messageHandler == null)
                throw new ArgumentNullException(nameof(messageHandler));

            _messageHandler = messageHandler;
        }

        public HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(_messageHandler, false);
        }

        public HttpMessageHandler CreateMessageHandler()
        {
            return new FakeHttpMessageHandler();
        }
    }
}