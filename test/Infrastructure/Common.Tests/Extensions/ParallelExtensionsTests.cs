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
        public class ValidationTestCase
        {
            public IEnumerable<int> Source { get; set; }

            public Func<int, int> Body { get; set; }
        }

        [UsedImplicitly]
        public static TheoryData<ValidationTestCase> ForEachAsyncParametersValidationTestsData;

        static ParallelExtensionsTests()
        {
            ForEachAsyncParametersValidationTestsData
                = new TheoryData<ValidationTestCase>
                  {
                      new ValidationTestCase {Source = null, Body = x => x},
                      new ValidationTestCase {Source = new[] {1, 2, 3}, Body = null}
                  };
        }

        [Theory]
        [MemberData(nameof(ForEachAsyncParametersValidationTestsData))]
        public void ForEachAsyncShouldValidateParameters(ValidationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.Source.ForEachAsync(testCase.Body, 1));
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