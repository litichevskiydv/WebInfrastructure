namespace Skeleton.Web.Integration.BaseApiClient.HttpClientFactories
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using Flurl.Http.Configuration;

    [ExcludeFromCodeCoverage]
    internal class HttpClientFactoryWithDecompressionEnabled : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler {AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate};
        }
    }
}