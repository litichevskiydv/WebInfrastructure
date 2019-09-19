namespace Skeleton.Web.Tests.Serialization.JsonNet
{
    using System;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Web.Serialization.JsonNet.Configuration;
    using Web.Serialization.JsonNet.JsonConverters;
    using Xunit;

    public class DateTimeConverterTests
    {
        #region TestCases

        public class DateTimeSerializationTestCase
        {
            public DateTime DateTime { get; set; }

            public string Expected { get; set; }
        }

        #endregion

        [UsedImplicitly]
        public static TheoryData<DateTimeSerializationTestCase> DateTimeSerializationTestCases;

        static DateTimeConverterTests()
        {
            DateTimeSerializationTestCases =
                new TheoryData<DateTimeSerializationTestCase>
                {
                    new DateTimeSerializationTestCase
                    {
                        DateTime = new DateTime(2016, 01, 01, 10, 30, 00, DateTimeKind.Utc),
                        Expected = "\"2016-01-01T10:30:00.0000000Z\""
                    },
                    new DateTimeSerializationTestCase
                    {
                        DateTime = new DateTime(2016, 01, 01, 10, 30, 00, DateTimeKind.Unspecified),
                        Expected = "\"2016-01-01T10:30:00.0000000Z\""
                    }
                };
        }

        [Theory]
        [MemberData(nameof(DateTimeSerializationTestCases))]
        public void ShouldSerializeDateTimeWithUtcOrUnspecifiedKindAsIs(DateTimeSerializationTestCase testCase)
        {
            // Given
            var serializerSettings = new JsonSerializerSettings().UseConverter(new DateTimeConverter());

            // When
            var actual = JsonConvert.SerializeObject(testCase.DateTime, serializerSettings);

            // Then
            Assert.Equal(testCase.Expected, actual);
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
