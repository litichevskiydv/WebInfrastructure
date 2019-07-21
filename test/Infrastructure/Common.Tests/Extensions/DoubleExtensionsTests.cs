namespace Skeleton.Common.Tests.Extensions
{
    using Common.Extensions;
    using JetBrains.Annotations;
    using Xunit;

    public class DoubleExtensionsTests
    {
        public class ComparisonOperationsTestCase
        {
            public double First { get; set; }

            public double Second { get; set; }

            public bool Expected { get; set; }
        }

        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> LessExtensionTestsData;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> LessOrEqualExtensionTestsData;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> GreaterExtensionTestsData;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> GreaterOrEqualExtensionTestsData;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> EqualExtensionTestsData;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> NotEqualExtensionTestsData;

        static DoubleExtensionsTests()
        {
            LessExtensionTestsData =
                new TheoryData<ComparisonOperationsTestCase>
                {
                    new ComparisonOperationsTestCase
                    {
                        First = 1d, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy / 2d, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d, Second = 1d, Expected = false
                    }
                };
            LessOrEqualExtensionTestsData =
                new TheoryData<ComparisonOperationsTestCase>
                {
                    new ComparisonOperationsTestCase
                    {
                        First = 1d, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy / 2d, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d + DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d, Second = 1d, Expected = false
                    }
                };
            GreaterExtensionTestsData =
                new TheoryData<ComparisonOperationsTestCase>
                {
                    new ComparisonOperationsTestCase
                    {
                        First = 1d, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d + DoubleExtensions.DefaultAccuracy / 2d, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d + DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d, Second = 1d, Expected = true
                    }
                };

            GreaterOrEqualExtensionTestsData =
                new TheoryData<ComparisonOperationsTestCase>
                {
                    new ComparisonOperationsTestCase
                    {
                        First = 1d, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy / 2d, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d + DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d, Second = 1d, Expected = true
                    }
                };
            EqualExtensionTestsData =
                new TheoryData<ComparisonOperationsTestCase>
                {
                    new ComparisonOperationsTestCase
                    {
                        First = 1d, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy / 2d, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d + DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d, Second = 1d, Expected = false
                    }
                };
            NotEqualExtensionTestsData =
                new TheoryData<ComparisonOperationsTestCase>
                {
                    new ComparisonOperationsTestCase
                    {
                        First = 1d, Second = 2d, Expected = true
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d - DoubleExtensions.DefaultAccuracy / 2d, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d + DoubleExtensions.DefaultAccuracy, Second = 2d, Expected = false
                    },
                    new ComparisonOperationsTestCase
                    {
                        First = 2d, Second = 1d, Expected = true
                    }
                };
        }

        [Theory]
        [MemberData(nameof(LessExtensionTestsData))]
        public static void ShouldCheckLessExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.Less(testCase.Second, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(LessOrEqualExtensionTestsData))]
        public static void ShouldCheckLessOrEqualExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.LessOrEqual(testCase.Second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(testCase.Expected, !testCase.Second.Less(testCase.First, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(GreaterExtensionTestsData))]
        public static void ShouldCheckGreaterExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.Greater(testCase.Second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(testCase.Expected, testCase.Second.Less(testCase.First, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(GreaterOrEqualExtensionTestsData))]
        public static void ShouldCheckGreaterOrEqualExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.GreaterOrEqual(testCase.Second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(testCase.Expected, !testCase.First.Less(testCase.Second, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(EqualExtensionTestsData))]
        public static void ShouldCheckEqualExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.Equal(testCase.Second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(
                testCase.Expected,
                !testCase.First.Less(testCase.Second, DoubleExtensions.DefaultAccuracy)
                && !testCase.Second.Less(testCase.First, DoubleExtensions.DefaultAccuracy)
            );
        }

        [Theory]
        [MemberData(nameof(NotEqualExtensionTestsData))]
        public static void ShouldCheckNotEqualExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.NotEqual(testCase.Second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(
                testCase.Expected,
                testCase.First.Less(testCase.Second, DoubleExtensions.DefaultAccuracy)
                || testCase.Second.Less(testCase.First, DoubleExtensions.DefaultAccuracy)
            );
        }
    }
}