namespace Skeleton.Web.Integration.BaseApiClient
{
    using Newtonsoft.Json;
    using Serialization;

    public class ClientConfiguration
    {
        public string BaseUrl { get; set; }

        public int TimeoutInMilliseconds { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }

        public ClientConfiguration()
        {
            SerializerSettings = new JsonSerializerSettings().UseDefaultSettings();
        }
    }
}