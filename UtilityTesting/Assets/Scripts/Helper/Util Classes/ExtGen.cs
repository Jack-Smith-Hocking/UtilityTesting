using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Helper
{
    public static class ExtGen
    {
        public static bool IsNull(this object obj) => obj == null || obj.Equals(null);
        public static bool IsNotNull(this object obj) => !obj.IsNull();

        public static bool IsEmpty<T>(this T[] collection) => collection.Length == 0;
        public static bool IsEmpty<T>(this List<T> collection) => collection.Count == 0;

        public static bool IsNotEmpty<T>(this T[] collection) => collection.Length > 0;
        public static bool IsNotEmpty<T>(this List<T> collection) => collection.Count > 0;

        public static void ToggleActive(this GameObject obj) => obj.SetActive(!obj.activeInHierarchy);

        public static bool TrySetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue val, bool overwrite = false)
        {
            if (!overwrite && dict.ContainsKey(key)) return false;

            dict[key] = val;
            return true;
        }
    }
}