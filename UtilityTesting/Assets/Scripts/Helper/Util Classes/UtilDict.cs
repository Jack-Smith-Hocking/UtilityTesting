using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Utility.Helper
{
    public static class UtilDict
    {
        /// <summary>
        /// Check if the collection at the key is valid, if it isn't then create a new collection at the key
        /// </summary>
        /// <param name="key">Key to check for valid collection</param>
        /// <param name="dict">Dictionary to validate</param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public static void ValidateCollectionValue<TKey, TValue>([NotNull] TKey key, [NotNull] Dictionary<TKey, TValue> dict)
            where TValue : ICollection, new()
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (dict.ContainsKey(key)) return;

            dict[key] = new TValue();
        }
    }
}