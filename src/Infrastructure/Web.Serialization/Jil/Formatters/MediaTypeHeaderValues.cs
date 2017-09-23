namespace Skeleton.Web.Serialization.Jil.Formatters
{
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;

    public class MediaTypeHeaderValues
    {
        public static readonly MediaTypeHeaderValue ApplicationJson = MediaTypeHeaderValue.Parse((StringSegment)"application/json").CopyAsReadOnly();
        public static readonly MediaTypeHeaderValue TextJson = MediaTypeHeaderValue.Parse((StringSegment)"text/json").CopyAsReadOnly();
        public static readonly MediaTypeHeaderValue ApplicationJsonPatch = MediaTypeHeaderValue.Parse((StringSegment)"application/json-patch+json").CopyAsReadOnly();
        public static readonly MediaTypeHeaderValue ApplicationAnyJsonSyntax = MediaTypeHeaderValue.Parse((StringSegment)"application/*+json").CopyAsReadOnly();
    }
}