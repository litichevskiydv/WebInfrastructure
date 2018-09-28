namespace Skeleton.Common.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Extensions;
    using JetBrains.Annotations;
    using Xunit;

    public class ParallelExtensionsTests
    {
        [UsedImplicitly]
        public static IEnumerable<object[]> ForEachAsyncParametersValidationTestsData;

        static ParallelExtensionsTests()
        {
            ForEachAsyncParametersValidationTestsData
                = new[]
                  {
                      new object[] {null, new Func<int, int>(x => x)},
                      new object[] {new[] {1, 2, 3}, null}
                  };
        }

        [Theory]
        [MemberData(nameof(ForEachAsyncParametersValidationTestsData))]
        public void ForEachAsyncShouldValidateParameters(IEnumerable<int> source, Func<int, int> body)
        {
            Assert.Throws<ArgumentNullException>(() => source.ForEachAsync(body, 1));
        }

        [Fact]
        public async Task ShouldExecuteForEachAsynchronously()
        {
            // Given
            var source = new[] {1, 2, 3};

            // When
            var actual = await Task.WhenAll(source.ForEachAsync(x => x * x, 1));

            // Then
            var expected = new[] {1, 4, 9};
            Assert.Equal(expected, actual);
        }
    }
}