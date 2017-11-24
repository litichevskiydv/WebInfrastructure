namespace Skeleton.Web.Tests.Serialization.Protobuf
{
    using System;
    using System.IO;
    using ProtoBuf.Meta;
    using Web.Serialization.Protobuf.Configuration;
    using Web.Serialization.Protobuf.Formatters.Surrogates;
    using Xunit;

    public class DateTimeOffsetSurrogateTests
    {
        [Fact]
        public void ShouldSerializeDateTimeOffset()
        {
            // Given
            var expected = new DateTimeOffset(2017, 10, 10, 00, 00, 00, TimeSpan.FromHours(3));

            // When
            DateTimeOffset actual;
            var typeModel = TypeModel.Create().WithTypeSurrogate<DateTimeOffset, DateTimeOffsetSurrogate>();
            using (var stream = new MemoryStream())
            {
                typeModel.Serialize(stream, expected);
                stream.Position = 0;
                actual = (DateTimeOffset)typeModel.Deserialize(stream, null, typeof(DateTimeOffset));
            }

            // Then
            Assert.Equal(expected, actual);
        }
    }
}