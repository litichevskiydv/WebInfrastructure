namespace Skeleton.Web.Serialization.Jil.Configuration
{
    using global::Jil;

    public static class OptionsExtensions
    {
        public static Options Default =>
            new Options(
                prettyPrint: false,
                excludeNulls: true,
                jsonp: false,
                dateFormat: DateTimeFormat.ISO8601,
                includeInherited: true,
                unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC,
                serializationNameFormat: SerializationNameFormat.CamelCase);
    }
}