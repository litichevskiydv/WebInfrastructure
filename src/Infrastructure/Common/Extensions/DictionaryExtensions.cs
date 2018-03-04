namespace Skeleton.Common.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out var value) ? value : default(TValue);
        }

        public static TValue GetOrEmpty<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            return dictionary.TryGetValue(key, out var value) ? value : new TValue();
        }

        public static TValue GetOrValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue replacement)
        {
            return dictionary.TryGetValue(key, out var value) ? value : replacement;
        }

        public static TValue GetOrValue<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary, 
            TKey key, 
            Func<TKey, TValue> valueFunc)
        {
            return dictionary.TryGetValue(key, out var value) ? value : valueFunc(key);
        }
    }
}