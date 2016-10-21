namespace Skeleton.Web.OutputFormatting.JsonConverters
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter()
        {
            DateTimeStyles = DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
            DateTimeFormat = "o";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string resultString;

            if (value is DateTime)
            {
                var dateTime = (DateTime)value;

                if ((DateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal && dateTime.Kind == DateTimeKind.Unspecified)
                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                if ((DateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal)
                    dateTime = dateTime.ToUniversalTime();

                resultString = dateTime.ToString(DateTimeFormat, Culture);
            }
            else
            {
                var dateTimeOffset = (DateTimeOffset)value;
                resultString = dateTimeOffset.ToString(DateTimeFormat, Culture);
            }

            writer.WriteValue(resultString);
        }
    }
}