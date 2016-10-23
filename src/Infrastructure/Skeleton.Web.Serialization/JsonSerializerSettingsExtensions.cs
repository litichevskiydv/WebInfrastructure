namespace Skeleton.Web.Serialization
{
    using JsonConverters;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings UseNullValueHandling(this JsonSerializerSettings serializerSettings, NullValueHandling option)
        {
            serializerSettings.NullValueHandling = option;
            return serializerSettings;
        }

        public static JsonSerializerSettings UseFormatting(this JsonSerializerSettings serializerSettings, Formatting option)
        {
            serializerSettings.Formatting = option;
            return serializerSettings;
        }

        public static JsonSerializerSettings UseContractResolver(this JsonSerializerSettings serializerSettings, IContractResolver contractResolver)
        {
            serializerSettings.ContractResolver = contractResolver;
            return serializerSettings;
        }

        public static JsonSerializerSettings UseConverter(this JsonSerializerSettings serializerSettings, JsonConverter convert)
        {
            serializerSettings.Converters.Add(convert);
            return serializerSettings;
        }

        public static JsonSerializerSettings UseDefaultSettings(this JsonSerializerSettings serializerSettings)
        {
            return serializerSettings
                .UseNullValueHandling(NullValueHandling.Ignore)
                .UseFormatting(Formatting.Indented)
                .UseContractResolver(new DefaultContractResolver())
                .UseConverter(new DateTimeConverter())
                .UseConverter(new StringEnumConverter());
        }
    }
}