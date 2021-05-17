using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper.Utility
{
    public static partial class Util
    {
        /// <summary>
        /// utility class for general functions
        /// </summary>
        public static class Gen
        {
            public static bool IsNull(object obj) => obj == null || obj.Equals(null);
            public static bool IsNotNull(object obj) => !IsNull(obj);

            public static bool IsEmpty<T>(T[] collection) => collection.Length == 0;
            public static bool IsEmpty<T>(List<T> collection) => collection.Count == 0;

            public static bool IsNotEmpty<T>(T[] collection) => collection.Length > 0;
            public static bool IsNotEmpty<T>(List<T> collection) => collection.Count > 0;

            public static T SetIfNull<T>(T obj, T value) where T : class => obj ?? value;
            public static void ThrowExceptionIfNull<T>(T obj) where T : class => _ = obj ?? throw new System.ArgumentNullException(nameof(obj));

            /// <summary>
            /// Check if the collection at the key is valid, if it isn't then create a new collection at the key
            /// </summary>
            /// <param name="key">Key to check for valid collection</param>
            /// <param name="dict">Dictionary to validate</param>
            /// <typeparam name="TKey"></typeparam>
            /// <typeparam name="TValue"></typeparam>
            public static void ValidateCollectionValue<TKey, TValue>(TKey key, Dictionary<TKey, TValue> dict) 
                where TValue : ICollection, new()
            {
                dict.ThrowExceptionIfNull();
                if (key == null) throw new ArgumentNullException(nameof(key));

                if (dict.ContainsKey(key)) return;

                dict[key] = new TValue();
            }

            // == EXPENSIVE == //
            // Only use to test while loop logic so Unity doesn't brick on you //
            // =============== //
            #region SafeWhile
            public static void SafeWhile(System.Func<bool> predicate, System.Action action, int maxIterations = 1000, bool logWarning = true, UnityEngine.Object logContext = null)
            {
                if (predicate.IsNull()) { Debug.LogWarning($"SafeWhile loop exited due to a null predicate", logContext); return; }
                if (action.IsNull()) { Debug.LogWarning($"SafeWhile loop exited due to a null action", logContext); return; }

                int _iterationCount = 0;

                while (predicate.Invoke() == true)
                {
                    action.Invoke();

                    _iterationCount++;

                    if (_iterationCount < maxIterations) continue;
                    if (logWarning) Debug.LogWarning($"SafeWhile loop broken out of, exceeded max iterations ({maxIterations})", logContext);

                    break;
                }
            }
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
    }
}