namespace Skeleton.Web.Serialization.ProtobufNet.Formatters
{
    using Microsoft.Net.Http.Headers;

    public class MediaTypeHeaderValues
    {
        public static readonly MediaTypeHeaderValue ApplicationProtobuf = MediaTypeHeaderValue.Parse("application/x-protobuf").CopyAsReadOnly();
        public static readonly MediaTypeHeaderValue ApplicationProtobufSynonym = MediaTypeHeaderValue.Parse("application/protobuf").CopyAsReadOnly();
    }
}