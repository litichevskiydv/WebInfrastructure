namespace Skeleton.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether sequence is null or contains no elements
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source" /></typeparam>
        /// <param name="source">Sequence for checking</param>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Any() == false;
        }

        /// <summary>
        /// Determines whether sequence isn't null and contains any elements
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source" /></typeparam>
        /// <param name="source">Sequence for checking</param>
        public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        /// <summary>
        /// Converts sequence to array
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source" /></typeparam>
        /// <param name="source">Sequence for conversion</param>
        /// <returns>Result array</returns>
        public static T[] AsArray<T>(this IEnumerable<T> source)
        {
            return source != null
                ? (source as T[] ?? source.ToArray())
                : new T[0];
        }

        /// <summary>
        /// Determines whether sequence <paramref name="first"/> equals to sequence <paramref name="second"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of sequences</typeparam>
        public static bool IsEquals<TSource>(
            this IEnumerable<TSource> first, 
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer = null)
        {
            if (ReferenceEquals(first, second))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;

            var equalityComparer = comparer ?? EqualityComparer<TSource>.Default;
            return first.SequenceEqual(second, equalityComparer);
        }

        /// <summary>
        /// Method for calculating collection <paramref name="items"/> hash code
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of sequences</typeparam>
        public static int GetHashCodeWithRespectToOrder<TSource>(
            this IEnumerable<TSource> items,
            Func<TSource, int> hashCodeCalculator = null)
        {
            int GetHashCode(TSource x) => hashCodeCalculator?.Invoke(x) ?? x.GetHashCode();
            unchecked
            {
                return items?.Aggregate(1, (hash, x) => (hash * 31) ^ GetHashCode(x)) ?? 0;
            }
        }

        /// <summary>
        /// Determines whether sequence <paramref name="first"/> is same as sequence <paramref name="second"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of sequences</typeparam>
        public static bool IsSame<TSource>(
            this IEnumerable<TSource> first, 
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer = null)
        {
            if (ReferenceEquals(first, second))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;

            var elementsCounts = first
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count(), comparer ?? EqualityComparer<TSource>.Default);
            foreach (var element in second)
            {
                if (elementsCounts.TryGetValue(element, out var elementCount) == false || elementCount == 0)
                    return false;

                elementsCounts[element]--;
            }

            return elementsCounts.All(x => x.Value == 0);
        }

        /// <summary>
        /// Method for calculating collection <paramref name="items"/> hash code
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of sequences</typeparam>
        public static int GetHashCodeWithoutRespectToOrder<TSource>(
            this IEnumerable<TSource> items,
            Func<TSource, int> hashCodeCalculator = null)
        {
            int GetHashCode(TSource x) => hashCodeCalculator?.Invoke(x) ?? x.GetHashCode();
            unchecked
            {
                return items
                           ?.Select(GetHashCode)
                           .OrderBy(x => x)
                           .Aggregate(1, (hash, x) => (hash * 31) ^ x)
                       ?? 0;
            }
        }
    }
}
