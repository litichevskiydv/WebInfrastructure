namespace Skeleton.Web.Integration.BaseApiClient.Serializers
{
    using System.IO;
    using Flurl.Http.Configuration;
    using Jil;

    public class JilJsonSerializer : ISerializer
    {
        private readonly Options _options;

        public JilJsonSerializer(Options options)
        {
            _options = options;
        }

        public string Serialize(object obj)
        {
            return JSON.Serialize(obj, _options);
        }

        public T Deserialize<T>(string s)
        {
            return JSON.Deserialize<T>(s, _options);
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
                return JSON.Deserialize<T>(reader, _options);
        }
    }
}