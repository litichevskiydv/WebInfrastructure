namespace Skeleton.Common.Tests.Extensions
{
    using System.Collections.Generic;
    using Common.Extensions;
    using JetBrains.Annotations;
    using Xunit;

    public class DoubleExtensionsTests
    {
        [UsedImplicitly]
        public static IEnumerable<object[]> LessExtensionTestsData;
        [UsedImplicitly]
        public static IEnumerable<object[]> LessOrEqualExtensionTestsData;
        [UsedImplicitly]
        public static IEnumerable<object[]> GreaterExtensionTestsData;
        [UsedImplicitly]
        public static IEnumerable<object[]> GreaterOrEqualExtensionTestsData;
        [UsedImplicitly]
        public static IEnumerable<object[]> EqualExtensionTestsData;
        [UsedImplicitly]
        public static IEnumerable<object[]> NotEqualExtensionTestsData;

        static DoubleExtensionsTests()
        {
            LessExtensionTestsData =
                new[]
                {
                    new object[] {1d, 2d, true},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy / 2d, 2d, false},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy, 2d, false},
                    new object[] {2d, 1d, false}
                };
            LessOrEqualExtensionTestsData =
                new[]
                {
                    new object[] {1d, 2d, true},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy, 2d, true},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy / 2d, 2d, true},
                    new object[] {2d + DoubleExtensions.DefaultAccuracy, 2d, true},
                    new object[] {2d, 1d, false}
                };
            GreaterExtensionTestsData =
                new[]
                {
                    new object[] {1d, 2d, false},
                    new object[] {2d + DoubleExtensions.DefaultAccuracy / 2d, 2d, false},
                    new object[] {2d + DoubleExtensions.DefaultAccuracy, 2d, false},
                    new object[] {2d, 1d, true}
                };

            GreaterOrEqualExtensionTestsData =
                new[]
                {
                    new object[] {1d, 2d, false},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy, 2d, true},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy / 2d, 2d, true},
                    new object[] {2d + DoubleExtensions.DefaultAccuracy, 2d, true},
                    new object[] {2d, 1d, true}
                };
            EqualExtensionTestsData =
                new[]
                {
                    new object[] {1d, 2d, false},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy, 2d, true},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy / 2d, 2d, true},
                    new object[] {2d, 2d, true},
                    new object[] {2d + DoubleExtensions.DefaultAccuracy, 2d, true},
                    new object[] {2d, 1d, false}
                };
            NotEqualExtensionTestsData =
                new[]
                {
                    new object[] {1d, 2d, true},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy, 2d, false},
                    new object[] {2d - DoubleExtensions.DefaultAccuracy / 2d, 2d, false},
                    new object[] {2d + DoubleExtensions.DefaultAccuracy, 2d, false},
                    new object[] {2d, 1d, true}
                };
        }

        [Theory]
        [MemberData(nameof(LessExtensionTestsData))]
        public static void ShouldCheckLessExtension(double first, double second, bool expected)
        {
            Assert.Equal(expected, first.Less(second, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(LessOrEqualExtensionTestsData))]
        public static void ShouldCheckLessOrEqualExtension(double first, double second, bool expected)
        {
            Assert.Equal(expected, first.LessOrEqual(second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(expected, !second.Less(first, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(GreaterExtensionTestsData))]
        public static void ShouldCheckGreaterExtension(double first, double second, bool expected)
        {
            Assert.Equal(expected, first.Greater(second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(expected, second.Less(first, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(GreaterOrEqualExtensionTestsData))]
        public static void ShouldCheckGreaterOrEqualExtension(double first, double second, bool expected)
        {
            Assert.Equal(expected, first.GreaterOrEqual(second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(expected, !first.Less(second, DoubleExtensions.DefaultAccuracy));
        }

        [Theory]
        [MemberData(nameof(EqualExtensionTestsData))]
        public static void ShouldCheckEqualExtension(double first, double second, bool expected)
        {
            Assert.Equal(expected, first.Equal(second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(
                expected,
                !first.Less(second, DoubleExtensions.DefaultAccuracy) && !second.Less(first, DoubleExtensions.DefaultAccuracy)
            );
        }

        [Theory]
        [MemberData(nameof(NotEqualExtensionTestsData))]
        public static void ShouldCheckNotEqualExtension(double first, double second, bool expected)
        {
            Assert.Equal(expected, first.NotEqual(second, DoubleExtensions.DefaultAccuracy));
            Assert.Equal(
                expected,
                first.Less(second, DoubleExtensions.DefaultAccuracy) || second.Less(first, DoubleExtensions.DefaultAccuracy)
            );
        }
    }
}