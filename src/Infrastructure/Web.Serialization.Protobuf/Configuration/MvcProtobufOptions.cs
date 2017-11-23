namespace Skeleton.Web.Serialization.Protobuf.Configuration
{
    using System;
    using ProtoBuf.Meta;

    public class MvcProtobufOptions
    {
        public Func<RuntimeTypeModel, RuntimeTypeModel> SerializerConfigurator { get; internal set; }
    }
}