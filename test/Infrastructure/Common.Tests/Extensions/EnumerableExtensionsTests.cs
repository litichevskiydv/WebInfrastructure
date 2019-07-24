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

        [UsedImplicitly]
        public static TheoryData<UnaryComparisonOperationsTestCase> IsEmptyExtensionTests;
        [UsedImplicitly]
        public static TheoryData<UnaryComparisonOperationsTestCase> IsNotEmptyExtensionTests;
        [UsedImplicitly]
        public static IEnumerable<object[]> AsArrayExtensionTests;
        [UsedImplicitly]
        public static TheoryData<BinaryComparisonOperationsTestCase> IsEqualsExtensionTests;
        [UsedImplicitly]
        public static TheoryData<BinaryComparisonOperationsTestCase> IsSameExtensionTests;

        static EnumerableExtensionsTests()
        {
            IsEmptyExtensionTests =
                new TheoryData<UnaryComparisonOperationsTestCase>
                {
                    new UnaryComparisonOperationsTestCase {Source = null, Expected = true},
                    new UnaryComparisonOperationsTestCase {Source = Enumerable.Empty<int>(), Expected = true},
                    new UnaryComparisonOperationsTestCase {Source = Enumerable.Repeat(1, 2), Expected = false}
                };
            IsNotEmptyExtensionTests =
                new TheoryData<UnaryComparisonOperationsTestCase>
                {
                    new UnaryComparisonOperationsTestCase {Source = null, Expected = false},
                    new UnaryComparisonOperationsTestCase {Source = Enumerable.Empty<int>(), Expected = false},
                    new UnaryComparisonOperationsTestCase {Source = Enumerable.Repeat(1, 2), Expected = true}
                };

            var arrayForTesting = new[] {1, 2, 3, 4, 5};
            AsArrayExtensionTests = new[]
                                    {
                                        new object[] {null, new int[0]},
                                        new object[] {Enumerable.Range(1, 3), new[] {1, 2, 3}},
                                        new object[] {arrayForTesting, arrayForTesting}
                                    };
            IsEqualsExtensionTests =
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
            IsSameExtensionTests =
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
        [MemberData(nameof(IsEmptyExtensionTests))]
        public void ShouldCheckCollectionsForEmptiness(UnaryComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.Source.IsEmpty());
        }

        [Theory]
        [MemberData(nameof(IsNotEmptyExtensionTests))]
        public void ShouldCheckCollectionsForNotEmptiness(UnaryComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.Source.IsNotEmpty());
        }

        [Theory]
        [MemberData(nameof(AsArrayExtensionTests))]
        public void ShouldCheckEnumerableToArrayConversion(IEnumerable<int> source, int[] expected)
        {
            Assert.Equal(expected, source.AsArray());
        }

        [Theory]
        [MemberData(nameof(IsEqualsExtensionTests))]
        public void ShouldCheckCollectionsIsEquals(BinaryComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.IsEquals(testCase.Second, EqualityComparer<int>.Default));
        }

        [Theory]
        [MemberData(nameof(IsEqualsExtensionTests))]
        public void ShouldCheckCollectionsHashCodeWithRespectToOrder(BinaryComparisonOperationsTestCase testCase)
        {
            Assert.True(
                testCase.First.IsEquals(testCase.Second)
                && testCase.First.GetHashCodeWithRespectToOrder(x => x) == testCase.Second.GetHashCodeWithRespectToOrder(x => x)
                || testCase.First.IsEquals(testCase.Second) == false
            );
        }

        [Theory]
        [MemberData(nameof(IsSameExtensionTests))]
        public void ShouldCheckCollectionsIsSame(BinaryComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.IsSame(testCase.Second, EqualityComparer<int>.Default));
        }

        [Theory]
        [MemberData(nameof(IsSameExtensionTests))]
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