namespace Skeleton.Common.Extensions
{
    using System;

    public static class DoubleExtensions
    {
        public const double DefaultAccuracy = 1e-7;

        private static double GetUsedAccuracy(double? accuracy) => accuracy ?? DefaultAccuracy;

        public static bool Equal(this double first, double second, double? accuracy = null)
        {
            var usedAccuracy = GetUsedAccuracy(accuracy);
            return first <= second + usedAccuracy
                   && first + usedAccuracy >= second;
        }

        public static bool NotEqual(this double first, double second, double? accuracy = null)
        {
            var usedAccuracy = GetUsedAccuracy(accuracy);
            return first + usedAccuracy < second
                || first > second + usedAccuracy;
        }

        public static bool LessOrEqual(this double first, double second, double? accuracy = null)
        {
            return first <= second + GetUsedAccuracy(accuracy);
        }

        public static bool Less(this double first, double second, double? accuracy = null)
        {
            return first + GetUsedAccuracy(accuracy) < second;
        }

        public static bool GreaterOrEqual(this double first, double second, double? accuracy = null)
        {
            return first + GetUsedAccuracy(accuracy) >= second;
        }

        public static bool Greater(this double first, double second, double? accuracy = null)
        {
            return first > second + GetUsedAccuracy(accuracy);
        }
    }
}