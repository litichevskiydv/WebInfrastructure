namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using System;
    using System.Net;
    using System.Net.Http;

    public static class HttpClientHandlerExtensions
    {
        public static HttpClientHandler WithAutomaticDecompression(this HttpClientHandler handler)
        {
            if(handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return handler;
        }
    }
}