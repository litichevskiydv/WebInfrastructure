namespace Skeleton.Web.Serialization.Protobuf.Serializer
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Abstractions;
    using Microsoft.Net.Http.Headers;
    using ProtoBuf.Meta;

    public class ProtobufSerializer : ISerializer
    {
        private readonly RuntimeTypeModel _runtimeTypeModel;

        public ProtobufSerializer(Func<RuntimeTypeModel, RuntimeTypeModel> serializerConfigurator)
        {
            if (serializerConfigurator == null)
                throw new ArgumentNullException(nameof(serializerConfigurator));

            _runtimeTypeModel = serializerConfigurator(TypeModel.Create());
        }

        public HttpContent Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                _runtimeTypeModel.Serialize(stream, obj);

                return new ByteArrayContent(stream.ToArray())
                       {
                           Headers = {ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(MediaType.ToString())}
                       };
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            return (T)_runtimeTypeModel.Deserialize(stream, null, typeof(T));
        }

        public MediaTypeHeaderValue MediaType => MediaTypeHeaderValues.ApplicationProtobuf;
    }
}