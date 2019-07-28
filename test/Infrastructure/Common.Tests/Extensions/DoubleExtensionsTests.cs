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
        public static TheoryData<ComparisonOperationsTestCase> LessExtensionTestCases;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> LessOrEqualExtensionTestCases;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> GreaterExtensionTestCases;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> GreaterOrEqualExtensionTestCases;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> EqualExtensionTestCases;
        [UsedImplicitly]
        public static TheoryData<ComparisonOperationsTestCase> NotEqualExtensionTestCases;

        static DoubleExtensionsTests()
        {
            LessExtensionTestCases =
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
            LessOrEqualExtensionTestCases =
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
            GreaterExtensionTestCases =
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

            GreaterOrEqualExtensionTestCases =
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
            EqualExtensionTestCases =
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
            NotEqualExtensionTestCases =
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
        [MemberData(nameof(LessExtensionTestCases))]
        public static void ShouldCheckLessExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.Less(testCase.Second, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(LessOrEqualExtensionTestCases))]
        public static void ShouldCheckLessOrEqualExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.LessOrEqual(testCase.Second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(testCase.Expected, !testCase.Second.Less(testCase.First, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(GreaterExtensionTestCases))]
        public static void ShouldCheckGreaterExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.Greater(testCase.Second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(testCase.Expected, testCase.Second.Less(testCase.First, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(GreaterOrEqualExtensionTestCases))]
        public static void ShouldCheckGreaterOrEqualExtension(ComparisonOperationsTestCase testCase)
        {
            Assert.Equal(testCase.Expected, testCase.First.GreaterOrEqual(testCase.Second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(testCase.Expected, !testCase.First.Less(testCase.Second, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(EqualExtensionTestCases))]
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
        [MemberData(nameof(NotEqualExtensionTestCases))]
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