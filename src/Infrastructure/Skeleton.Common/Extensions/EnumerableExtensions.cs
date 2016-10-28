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
        /// Determines whether sequence <paramref name="first"/> equals to sequence <paramref name="second"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of sequences</typeparam>
        public static bool EqualsByElements<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (ReferenceEquals(first, second))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;
            return first.SequenceEqual(second);
        }
    }
}