using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper.Utility
{
    public static class ExtGen
    {
        public static bool IsNull(this object obj) => obj is null || obj.Equals(null);
        public static bool IsNotNull(this object obj) => !obj.IsNull();

        public static bool IsEmpty<T>(this T[] collection) => collection.Length == 0;
        public static bool IsEmpty<T>(this List<T> collection) => collection.Count == 0;

        public static bool IsNotEmpty<T>(this T[] collection) => collection.Length > 0;
        public static bool IsNotEmpty<T>(this List<T> collection) => collection.Count > 0;

        public static T SetIfNull<T>(this T obj, T value) where T : class => obj ?? value;
        public static void ThrowExceptionIfNull<T>(this T obj) where T : class => _ = obj ?? throw new System.ArgumentNullException(nameof(obj));

        public static bool DoesNotContainKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) => dict.ContainsKey(key) == false;
        public static bool DoesNotContainValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value) => dict.ContainsValue(value) == false;

        public static bool TrySetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue val, bool overwrite = false)
        {
            if (!overwrite && dict.ContainsKey(key)) return false;

            dict[key] = val;
            return true;
        }
    }
}