namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using System;
    using Serialization.Abstractions;

    public class BaseClientOptions
    {
        public string BaseUrl { get; set; }

        public TimeSpan Timeout { get; set; }

        public ISerializer Serializer { get; set; }
    }
}