namespace Skeleton.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Serialization.JsonConverters;
    using Xunit;
    using Serialization;

    public class DateTimeConverterTests
    {
        [UsedImplicitly]
        public static IEnumerable<object[]> DateTimeSerializationTestsData;

        static DateTimeConverterTests()
        {
            DateTimeSerializationTestsData =
                new[]
                {
                    new object[] {new DateTime(2016, 01, 01, 10, 30, 00, DateTimeKind.Utc), "\"2016-01-01T10:30:00.0000000Z\""},
                    new object[] {new DateTime(2016, 01, 01, 10, 30, 00, DateTimeKind.Unspecified), "\"2016-01-01T10:30:00.0000000Z\""}
                };
        }

        [Theory]
        [MemberData(nameof(DateTimeSerializationTestsData))]
        public void ShouldSerializeDateTimeWithUtcOrUnspecifiedKindAsIs(DateTime dateTime, string expected)
        {
            // Given
            var serializerSettings = new JsonSerializerSettings().UseConverter(new DateTimeConverter());

            // When
            var actual = JsonConvert.SerializeObject(dateTime, serializerSettings);

            // Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldAdjustLocalDateTimeBeforeSerialization()
        {
            // Given
            var dateTime = new DateTime(2016, 01, 01, 10, 30, 00, DateTimeKind.Local);
            var serializerSettings = new JsonSerializerSettings().UseConverter(new DateTimeConverter());

            // When
            var actual = JsonConvert.SerializeObject(dateTime, serializerSettings);
            var expected = JsonConvert.SerializeObject(dateTime.ToUniversalTime(), serializerSettings);

            // Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldSerializeDateTimeOffset()
        {
            // Given
            var dateTimeOffset = new DateTimeOffset(2016, 01, 01, 10, 30, 00, TimeSpan.FromHours(3));
            var serializerSettings = new JsonSerializerSettings().UseConverter(new DateTimeConverter());

            // When
            var actual = JsonConvert.SerializeObject(dateTimeOffset, serializerSettings);

            // Then
            Assert.Equal("\"2016-01-01T10:30:00.0000000+03:00\"", actual);
        }
    }
}
