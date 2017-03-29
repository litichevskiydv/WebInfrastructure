namespace Skeleton.Common.Tests.Extensions
{
    using System.ComponentModel;
    using Common.Extensions;
    using Xunit;

    public class EnumExtensionsTests
    {
        private enum Marks
        {
            [Description("Bad result")]
            One
        }

        [Fact]
        public void ShouldGetDescriptionForEnumMember()
        {
            //Given
            const string expectedDescription = "Bad result";

            // When
            var actualDescription = Marks.One.GetDescription();

            // Then
            Assert.Equal(expectedDescription, actualDescription);
        }
    }
}