namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using System;
    using System.Net.Http;
    using Serialization.Abstractions;

    public class ClientConfiguration
    {
        public string BaseUrl { get; set; }

        public TimeSpan Timeout { get; set; }

        public ISerializer Serializer { get; set; }

        public Func<HttpMessageHandler> HttpMessageHandlersFactory { get; set; }

        public bool DisposeHttpMessageHandlersAfterCall { get; set; }
    }
}