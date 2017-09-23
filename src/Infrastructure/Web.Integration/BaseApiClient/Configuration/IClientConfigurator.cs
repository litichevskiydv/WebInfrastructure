namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using System;
    using System.Net.Http;
    using Flurl.Http.Configuration;

    public interface IClientConfigurator
    {
        IClientConfigurator WithBaseUrl(string baseUrl);

        IClientConfigurator WithTimeout(TimeSpan timeout);

        IClientConfigurator WithSerializer(ISerializer serializer);

        IClientConfigurator WithHttpMessageHandler(HttpMessageHandler messageHandler);
    }
}