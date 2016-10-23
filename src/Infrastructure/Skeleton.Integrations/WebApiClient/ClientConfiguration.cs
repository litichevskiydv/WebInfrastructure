namespace Skeleton.Integrations.WebApiClient
{
    using Newtonsoft.Json;
    using Web.Serialization;

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