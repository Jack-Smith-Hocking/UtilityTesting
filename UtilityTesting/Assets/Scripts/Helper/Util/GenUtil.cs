using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jack.Utility
{
    /// <summary>
    /// utility class for general functions
    /// </summary>
    public static partial class GenUtil
    {
        /// <summary>
        /// Check if the collection at the key is valid, if it isn't then create a new collection at the key
        /// </summary>
        /// <param name="key">Key to check for valid collection</param>
        /// <param name="dict">Dictionary to validate</param>
        public static void ValidateCollectionValue<TKey, TValue>(TKey key, Dictionary<TKey, TValue> dict) where TValue : ICollection, new()
        {
            if (dict.ContainsKey(key) && dict[key].IsNotNull()) return;

            dict[key] = new TValue();
        }

        /// <summary>
        /// Return true if the obj is null
        /// </summary>
        public static bool IsNull(object obj) => obj == null || obj.Equals(null);
        /// <summary>
        /// Return true if the obj is not null
        /// </summary>
        public static bool IsNotNull(object obj) => !IsNull(obj);

        /// <summary>
        /// Return true if there are no elements in the collection
        /// </summary>
        public static bool IsEmpty<T>(ICollection<T> collection) => collection.Count == 0;
        /// <summary>
        /// Return true if there are elements in the collection
        /// </summary>
        public static bool IsNotEmpty<T>(ICollection<T> collection) => !IsEmpty(collection);

        /// <summary>
        /// Convert a Dictionary to a ICollection of KeyValuePairs, uses a cast -> "Dictionary as ICollection"
        /// </summary>
        public static ICollection<KeyValuePair<TKey, TValue>> Flatten<TKey, TValue>(Dictionary<TKey, TValue> dict) => dict as ICollection<KeyValuePair<TKey, TValue>>;

        /// <summary>
        /// If the obj is null, return value
        /// </summary>
        public static T SetIfNull<T>(T obj, T value) where T : class => obj ?? value;

        // == EXPENSIVE == //
        // Only use to test while loop logic so Unity doesn't brick on you //
        // =============== //
        #region SafeWhile
        /// <summary>
        /// A safe while loop with a break out counter to avoid infinite looping
        /// </summary>
        /// <param name="predicate">The logic to determine if the loop should continue</param>
        /// <param name="action">The action to be performed in the loop</param>
        /// <param name="maxIterations">The max loop count before being considered an error</param>
        /// <param name="logWarning">Whether to log a warning when the loop is broken out of</param>
        public static void SafeWhile(System.Func<bool> predicate, System.Action action, int maxIterations = 1000, bool logWarning = true, UnityEngine.Object logContext = null)
        {
            if (predicate.IsNull()) { Debug.LogWarning($"SafeWhile loop exited due to a null predicate", logContext); return; }
            if (action.IsNull()) { Debug.LogWarning($"SafeWhile loop exited due to a null action", logContext); return; }

            SafeWhile(() =>
            {
                bool _valid = predicate.Invoke();

                if (_valid) action?.Invoke();

                return _valid;
            }, maxIterations, logWarning, logContext);
        }

        /// <summary>
        /// A safe while loop with a break out counter to avoid infinite looping
        /// </summary>
        /// <param name="loopContent">A combination of the predicate (loop logic) and the action to perform in the loop</param>
        /// <param name="maxIterations">The max loop count before being considered an error</param>
        /// <param name="logWarning">Whether to log a warning when the loop is broken out of</param>
        public static void SafeWhile(System.Func<bool> loopContent, int maxIterations = 1000, bool logWarning = true, UnityEngine.Object logContext = null)
        {
            if (loopContent.IsNull()) { Debug.LogWarning($"SafeWhile loop exited due to null {nameof(loopContent)}", logContext); return; }

            int _iterationCount = 0;

            while (loopContent.Invoke() == true)
            {
                _iterationCount++;

                if (_iterationCount < maxIterations) continue;
                if (logWarning) Debug.LogWarning($"SafeWhile loop broken out of, exceeded max iterations ({maxIterations})", logContext);

                break;
            }
        }
        #endregion
    }


    public static partial class Gen_Ext
    {
        public static void OnAll<T>(this ICollection<T> collection, System.Action<T> action)
        {
            foreach (var _item in collection) action.Invoke(_item);
        }

        /// <summary>
        /// Return true if the obj is null
        /// </summary>
        /// <returns></returns>
        public static bool IsNull(this object obj) => obj is null || obj.Equals(null);
        /// <summary>
        /// Return true if the obj is not null
        /// </summary>
        /// <returns></returns>
        public static bool IsNotNull(this object obj) => !obj.IsNull();

        /// <summary>
        /// Return true if there are no elements in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this ICollection<T> collection) => collection.Count == 0;
        /// <summary>
        /// Return true if there are elements in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(this ICollection<T> collection) => collection.Count > 0;

        /// <summary>
        /// Return true if the key is not found in the dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DoesNotContainKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) => dict.ContainsKey(key) == false;
        /// <summary>
        /// Return true if the value is not found in the dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool DoesNotContainValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value) => dict.ContainsValue(value) == false;

        /// <summary>
        /// Attempt to set a value in a dictionary
        /// </summary>
        /// <param name="key">Key to set at</param>
        /// <param name="val">Value to set to</param>
        /// <param name="overwrite">Whether to overwrite present value</param>
        public static bool TrySetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue val, bool overwrite = false)
        {
            if (!overwrite && dict.ContainsKey(key)) return false;

            dict[key] = val;
            return true;
        }

        /// <summary>
        /// Convert an ICollection of one type to an ICollection of another type, given a conversion method
        /// </summary>
        /// <param name="conversion">The method to convert from TOrigin to TConvert</param>
        public static ICollection<TConvert> Convert<TOrigin, TConvert>(this ICollection<TOrigin> collection, System.Func<TOrigin, TConvert> conversion)
        {
            List<TConvert> _convertedList = new List<TConvert>(collection.Count);
            collection.OnAll(elem => _convertedList.Add(conversion.Invoke(elem)));

            return _convertedList as ICollection<TConvert>;
        }
        /// <summary>
        /// Sort an ICollection by converting it to a list and calling the Sort method
        /// </summary>
        public static ICollection<T> Sort<T>(this ICollection<T> collection, Comparison<T> comparison)
        {
            List<T> _list = collection.ToList();
            _list.Sort(comparison);

            return _list as ICollection<T>;
        }

        /// <summary>
        /// Convert a Dictionary to a ICollection of KeyValuePairs, uses a cast -> "Dictionary as ICollection"
        /// </summary>
        public static ICollection<KeyValuePair<TKey, TValue>> Flatten<TKey, TValue>(this Dictionary<TKey, TValue> dict) => dict as ICollection<KeyValuePair<TKey, TValue>>;

        /// <summary>
        /// If the obj is null, return value
        /// </summary>
        public static T SetIfNull<T>(this T obj, T value) where T : class => obj ?? value;
    }
}