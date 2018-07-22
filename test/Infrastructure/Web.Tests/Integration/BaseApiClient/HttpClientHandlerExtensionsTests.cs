namespace Skeleton.Web.Tests.Integration.BaseApiClient
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Web.Integration.BaseApiClient.Configuration;
    using Xunit;

    public class HttpClientHandlerExtensionsTests
    {
        [Fact]
        public void WithAutomaticDecompressionShouldValidateParameters()
        {
            Assert.Throws<ArgumentNullException>(() => HttpClientHandlerExtensions.WithAutomaticDecompression(null));
        }

        [Fact]
        public void ShouldChangeCompressionSettings()
        {
            // Given
            var handler = new HttpClientHandler();

            // When
            handler.WithAutomaticDecompression();

            // Then
            Assert.Equal(DecompressionMethods.Deflate | DecompressionMethods.GZip, handler.AutomaticDecompression);
        }
    }
}