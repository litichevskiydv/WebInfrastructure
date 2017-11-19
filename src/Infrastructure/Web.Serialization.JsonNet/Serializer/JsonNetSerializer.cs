namespace Skeleton.Web.Serialization.JsonNet.Serializer
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using Abstractions;
    using Microsoft.Net.Http.Headers;
    using Newtonsoft.Json;

    public class JsonNetSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public JsonNetSerializer(JsonSerializerSettings settings)
        {
            if(settings == null)
                throw new ArgumentNullException(nameof(settings));
            _settings = settings;

            MediaType = MediaTypeHeaderValue.Parse("application/json").CopyAsReadOnly();
        }

        public HttpContent Serialize(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj, _settings), Encoding.UTF8, MediaType.MediaType.ToString());
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
                return JsonSerializer.CreateDefault(_settings).Deserialize<T>(jsonTextReader);
        }

        public MediaTypeHeaderValue MediaType { get; }
    }
}