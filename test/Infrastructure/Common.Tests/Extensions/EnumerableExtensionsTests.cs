namespace Skeleton.Common.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;
    using JetBrains.Annotations;
    using Xunit;

    public class EnumerableExtensionsTests
    {
        public class UnaryComparisonOperationsTestCase
        {
            public IEnumerable<int> Source { get; set; }

            public bool Expected { get; set; }
        }

        public class BinaryComparisonOperationsTestCase
        {
            public IEnumerable<int> First { get; set; }

            public IEnumerable<int> Second { get; set; }

            public bool Expected { get; set; }
        }

        public class AsArrayExtensionTestCase
        {
            public IEnumerable<int> Source { get; set; }

            public int[] Expected { get; set; }
        }

        [UsedImplicitly]
        public static TheoryData<UnaryComparisonOperationsTestCase> IsEmptyExtensionTestCases;
        [UsedImplicitly]
        public static TheoryData<UnaryComparisonOperationsTestCase> IsNotEmptyExtensionTestCases;
        [UsedImplicitly]
        public static TheoryData<AsArrayExtensionTestCase> AsArrayExtensionTestCases;
        [UsedImplicitly]
        public static TheoryData<BinaryComparisonOperationsTestCase> IsEqualsExtensionTestCases;
        [UsedImplicitly]
        public static TheoryData<BinaryComparisonOperationsTestCase> IsSameExtensionTestCases;

        static EnumerableExtensionsTests()
        {
            IsEmptyExtensionTestCases =
                new TheoryData<UnaryComparisonOperationsTestCase>
                {
                    new UnaryComparisonOperationsTestCase {Source = null, Expected = true},
                    new UnaryComparisonOperationsTestCase {Source = Enumerable.Empty<int>(), Expected = true},
                    new UnaryComparisonOperationsTestCase {Source = Enumerable.Repeat(1, 2), Expected = false}
                };
            IsNotEmptyExtensionTestCases =
                new TheoryData<UnaryComparisonOperationsTestCase>
                {
                    new UnaryComparisonOperationsTestCase {Source = null, Expected = false},
                    new UnaryComparisonOperationsTestCase {Source = Enumerable.Empty<int>(), Expected = false},
                    new UnaryComparisonOperationsTestCase {Source = Enumerable.Repeat(1, 2), Expected = true}
                };

            var arrayForTesting = new[] {1, 2, 3, 4, 5};
            AsArrayExtensionTestCases =
                new TheoryData<AsArrayExtensionTestCase>
                {
                    new AsArrayExtensionTestCase {Source = null, Expected = new int[0]},
                    new AsArrayExtensionTestCase {Source = Enumerable.Range(1, 3), Expected = new[] {1, 2, 3}},
                    new AsArrayExtensionTestCase {Source = arrayForTesting, Expected = arrayForTesting}
                };

            IsEqualsExtensionTestCases =
                new TheoryData<BinaryComparisonOperationsTestCase>
                {
                    new BinaryComparisonOperationsTestCase
                    {
                        First = null, Second = null, Expected = true
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = Enumerable.Empty<int>(), Second = Enumerable.Empty<int>(), Expected = true
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = null, Second = Enumerable.Empty<int>(), Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = Enumerable.Empty<int>(), Second = null, Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = Enumerable.Empty<int>(), Second = Enumerable.Repeat(1, 2), Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = Enumerable.Repeat(1, 2), Second = Enumerable.Repeat(1, 2), Expected = true
                    }
                };

            var array = new[] {1, 2, 3};
            IsSameExtensionTestCases =
                new TheoryData<BinaryComparisonOperationsTestCase>
                {
                    new BinaryComparisonOperationsTestCase
                    {
                        First = null, Second = null, Expected = true
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = Enumerable.Empty<int>(), Second = Enumerable.Empty<int>(), Expected = true
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = null, Second = Enumerable.Empty<int>(), Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = Enumerable.Empty<int>(), Second = null, Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = Enumerable.Empty<int>(), Second = Enumerable.Repeat(1, 2), Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = new[] {1, 2}, Second = new[] {1, 2}, Expected = true
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = new[] {2, 1}, Second = new[] {1, 2}, Expected = true
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = new[] {1, 1}, Second = new[] {1, 2}, Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = new[] {1, 2}, Second = new[] {1, 1}, Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = new[] {1, 1}, Second = new[] {1, 1}, Expected = true
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = new[] {1, 1}, Second = new[] {1, 1, 1}, Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = new[] {1, 1, 2, 2}, Second = new[] {1, 1, 1, 2}, Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = new[] {1, 1, 1, 2}, Second = new[] {1, 1, 2, 2}, Expected = false
                    },
                    new BinaryComparisonOperationsTestCase
                    {
                        First = array, Second = array, Expected = true
                    }
                };
        }

        [Theory]
        [MemberData(nameof(IsEmptyExtensionTestCases))]
        public void ShouldCheckCollectionsForEmptiness(UnaryComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.Source.IsEmpty());
        }

        [Theory]
        [MemberData(nameof(IsNotEmptyExtensionTestCases))]
        public void ShouldCheckCollectionsForNotEmptiness(UnaryComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.Source.IsNotEmpty());
        }

        [Theory]
        [MemberData(nameof(AsArrayExtensionTestCases))]
        public void ShouldCheckEnumerableToArrayConversion(AsArrayExtensionTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.Source.AsArray());
        }

        [Theory]
        [MemberData(nameof(IsEqualsExtensionTestCases))]
        public void ShouldCheckCollectionsIsEquals(BinaryComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.IsEquals(testCase.Second, EqualityComparer<int>.Default));
        }

        [Theory]
        [MemberData(nameof(IsEqualsExtensionTestCases))]
        public void ShouldCheckCollectionsHashCodeWithRespectToOrder(BinaryComparisonOperationsTestCase testCase)
        {
            Assert.True(
                testCase.First.IsEquals(testCase.Second)
                && testCase.First.GetHashCodeWithRespectToOrder(x => x) == testCase.Second.GetHashCodeWithRespectToOrder(x => x)
                || testCase.First.IsEquals(testCase.Second) == false
            );
        }

        [Theory]
        [MemberData(nameof(IsSameExtensionTestCases))]
        public void ShouldCheckCollectionsIsSame(BinaryComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.IsSame(testCase.Second, EqualityComparer<int>.Default));
        }

        [Theory]
        [MemberData(nameof(IsSameExtensionTestCases))]
        public void ShouldCheckCollectionsHashCodeWithoutRespectToOrder(BinaryComparisonOperationsTestCase testCase)
        {

            Assert.True(
                testCase.First.IsSame(testCase.Second)
                && testCase.First.GetHashCodeWithoutRespectToOrder(x => x) == testCase.Second.GetHashCodeWithoutRespectToOrder(x => x)
                || testCase.First.IsSame(testCase.Second) == false
            );
        }
    }
}