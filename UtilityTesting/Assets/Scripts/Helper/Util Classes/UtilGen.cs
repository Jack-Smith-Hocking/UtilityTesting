using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Helper
{
    public static class UtilGen
    {
        public static bool IsNUll(object obj) => obj == null || obj.Equals(null);
        public static bool IsNotNull(object obj) => !IsNUll(obj);

        public static bool IsEmpty<T>(T[] collection) => collection.Length == 0;
        public static bool IsEmpty<T>(List<T> collection) => collection.Count == 0;

        public static bool IsNotEmpty<T>(T[] collection) => collection.Length > 0;
        public static bool IsNotEmpty<T>(List<T> collection) => collection.Count > 0;

        // == EXPENSIVE == //
        // Only use to test while loop logic so Unity doesn't brick on you //
        // =============== //
        #region SafeWhile
        public static void SafeWhile(System.Func<bool> predicate, System.Action action, int maxIterations = 1000, bool logWarning = true, UnityEngine.Object logContext = null)
        {
            if (predicate == null) { Debug.LogWarning($"SafeWhile loop exited due to a null predicate", logContext); return; }
            if (action == null) { Debug.LogWarning($"SafeWhile loop exited due to a null action", logContext); return; }

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
            if (loopContent == null) { Debug.LogWarning($"SafeWhile loop exited due to null {nameof(loopContent)}", logContext); return; }

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