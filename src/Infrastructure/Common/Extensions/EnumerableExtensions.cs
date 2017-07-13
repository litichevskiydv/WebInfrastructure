namespace Skeleton.Common.Extensions
{
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
        /// <param name="source">Sequence for convertation</param>
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
        public static bool IsEquals<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (ReferenceEquals(first, second))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;

            return first.SequenceEqual(second);
        }

        /// <summary>
        /// Determines whether sequence <paramref name="first"/> is same as sequence <paramref name="second"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of sequences</typeparam>
        public static bool IsSame<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (ReferenceEquals(first, second))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;

            var firstArray = first as TSource[] ?? first.ToArray();
            var secondArray = second as TSource[] ?? second.ToArray();
            return firstArray.SequenceEqual(secondArray)
                   || firstArray.Length == secondArray.Length && firstArray.Except(secondArray).Any() == false;
        }

        /// <summary>
        /// Method for calculating collection <paramref name="items"/> hash code
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of sequences</typeparam>
        public static int GetCollectionHashCode<TSource>(this IEnumerable<TSource> items)
        {
            return items?.Aggregate(1, (hash, x) => hash * 31 ^ x.GetHashCode()) ?? 0;
        }
    }
}