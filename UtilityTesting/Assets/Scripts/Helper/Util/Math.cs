using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace Jack.Utility
{
    public static partial class Util
    {
        public static class Math
        {
            /// <summary>
            /// Rotate a vector by an angle given an axis
            /// </summary>
            /// <param name="angle">Angle (in degrees) to rotate by</param>
            /// <param name="axis">The axis to rotate around</param>
            /// <param name="dir">THe direction to rotate</param>
            /// <returns>A rotated vector</returns>
            public static Vector3 RotateBy(float angle, Vector3 axis, Vector3 dir) => Quaternion.AngleAxis(angle, axis.normalized) * dir.normalized;

            /// <summary>
            /// Convert a dot product value to radians
            /// </summary>
            public static float DotToRad(float dotProduct) => Mathf.Acos(dotProduct);
            /// <summary>
            /// Convert a dot product value to degrees
            /// </summary>
            public static float DotToDegree(float dotProduct) => Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

            /// <summary>
            /// Check if a layer is set in a bit mask
            /// </summary>
            public static bool InLayerMask(int layer, int mask) => layer == (layer | (1 << mask));
            /// <summary>
            /// Check if a layer is set in a bit mask
            /// </summary>
            public static bool InLayerMask(int layer, LayerMask mask) => InLayerMask(layer, mask.value);

            /// <summary>
            /// Return true if value is >= min and <= max
            /// </summary>
            /// <param name="value"></param>
            /// <param name="min">[inclusive]</param>
            /// <param name="max">[inclusive]</param>
            /// <returns></returns>
            public static bool InRange(int value, int min, int max) => value >= min && value <= max;
            /// <summary>
            /// Return true if value is >= min and <= max
            /// </summary>
            /// <param name="value"></param>
            /// <param name="min">[inclusive]</param>
            /// <param name="max">[inclusive]</param>
            /// <returns></returns>
            public static bool InRange(float value, float min, float max) => value >= min && value <= max;

            #region Direction
            /// <summary>
            /// Returns direction from start to end
            /// </summary>
            public static Vector3 Direction(Vector3 start, Vector3 end, bool norm = true) => norm ? (end - start).normalized : (end - start);
            /// <summary>
            /// Returns direction from start to end
            /// </summary>
            public static Vector3 Direction(Transform start, Transform end, bool norm = true) => Direction(start.position, end.position, norm);
            /// <summary>
            /// Returns direction from start to end
            /// </summary>
            public static Vector3 Direction(GameObject start, GameObject end, bool norm = true) => Direction(start.transform.position, end.transform.position, norm);
            #endregion

            #region Distance Squared
            /// <summary>
            /// Return the square magnitude of two Vector3s
            /// </summary>
            public static float DistanceSqrd(Vector3 a, Vector3 b) => (a - b).sqrMagnitude;
            /// <summary>
            /// Return the square magnitude of two Transforms
            /// </summary>
            public static float DistanceSqrd(Transform a, Transform b) => DistanceSqrd(a.position, b.position);
            /// <summary>
            /// Return the square magnitude of two GameObjects
            /// </summary>
            public static float DistanceSqrd(GameObject a, GameObject b) => DistanceSqrd(a.transform.position, b.transform.position);
            #endregion

            #region InDistance
            /// <summary>
            /// Check if two Vector3s are within 'dist' of each other
            /// </summary>
            /// <param name="dist">[inclusive]</param>
            public static bool InDistance(Vector3 a, Vector3 b, float dist) => (a - b).sqrMagnitude <= (dist * dist);
            /// <summary>
            /// Check if two Transforms are within 'dist' of each other
            /// </summary>
            /// <param name="dist">[inclusive]</param>
            public static bool InDistance(Transform a, Transform b, float dist) => InDistance(a.position, b.position, dist);
            /// <summary>
            /// Check if to GameObjects are within 'dist' of each other
            /// </summary>
            /// <param name="dist">[inclusive]</param>
            public static bool InDistance(GameObject a, GameObject b, float dist) => InDistance(a.transform.position, b.transform.position, dist);
            #endregion

            #region OutDistance
            /// <summary>
            /// Check if two Vector3s are further than 'dist' from each other
            /// </summary>
            /// <param name="dist">[exclusive]</param>
            public static bool OutDistance(Vector3 a, Vector3 b, float dist) => (a - b).sqrMagnitude <= (dist * dist);
            /// <summary>
            /// Check if two Transforms are further than 'dist' from each other
            /// </summary>
            /// <param name="dist">[exclusive]</param>
            public static bool OutDistance(Transform a, Transform b, float dist) => OutDistance(a.position, b.position, dist);
            /// <summary>
            /// Check if two GameObjects are further than 'dist' from each other
            /// </summary>
            /// <param name="dist">[exclusive]</param>
            public static bool OutDistance(GameObject a, GameObject b, float dist) => OutDistance(a.transform.position, b.transform.position, dist);
            #endregion
        }
    }

    public static class ExtMath
    {
        /// <summary>
        /// Return the value squared
        /// </summary>
        public static int Sqrd(this int num) => (num * num);
        /// <summary>
        /// Return the value squared
        /// </summary>
        public static float Sqrd(this float num) => (num * num);

        /// <summary>
        /// Convert a bool to an int
        /// </summary>
        public static int ToInt(this bool boolean) => boolean ? 1 : 0;

        /// <summary>
        /// Check if a layer is set in a bit mask
        /// </summary>
        public static bool InLayerMask(this int layer, int mask) => layer == (layer | (1 << mask));
        /// <summary>
        /// Check if a layer is set in a bit mask
        /// </summary>
        public static bool InLayerMask(this int layer, LayerMask mask) => InLayerMask(layer, mask.value);

        /// <summary>
        /// Return true if value is >= min and <= max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min">[inclusive]</param>
        /// <param name="max">[inclusive]</param>
        /// <returns></returns>
        public static bool InRange(this int value, int min, int max) => value >= min && value <= max;
        /// <summary>
        /// Return true if value is >= min and <= max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min">[inclusive]</param>
        /// <param name="max">[inclusive]</param>
        /// <returns></returns>
        public static bool InRange(this float value, float min, float max) => value >= min && value <= max;

        /// <summary>
        /// Calculate a point on a ray, origin + (direction * dist)
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="dist">Distance along the ray</param>
        /// <returns>A point in the direction of the ray, 'dist' units from the origin</returns>
        public static Vector3 Point(this Ray ray, float dist) => ray.origin + (ray.direction.normalized * dist);

        #region DirectionTo
        /// <summary>
        /// Return the direction to end
        /// </summary>
        /// <param name="norm">Whether to normalise the direction</param>
        public static Vector3 DirectionTo(this Vector3 start, Vector3 end, bool norm = true) => DirectionTo(start, end, norm);
        /// <summary>
        /// Return the direction to end
        /// </summary>
        /// <param name="norm">Whether to normalise the direction</param>
        public static Vector3 DirectionTo(this Transform start, Transform end, bool norm = true) => DirectionTo(start, end, norm);
        /// <summary>
        /// Return the direction to end
        /// </summary>
        /// <param name="norm">Whether to normalise the direction</param>
        public static Vector3 DirectionTo(this GameObject start, GameObject end, bool norm = true) => DirectionTo(start, end, norm);
        #endregion

        #region Distance Squared
        /// <summary>
        /// Return the square magnitude of two Vector3s
        /// </summary>
        public static float DistanceSqrd(this Vector3 a, Vector3 b) => DistanceSqrd(a, b);
        /// <summary>
        /// Return the square magnitude of two Transforms
        /// </summary>
        public static float DistanceSqrd(this Transform a, Transform b) => DistanceSqrd(a, b);
        /// <summary>
        /// Return the square magnitude of two GameObjects
        /// </summary>
        public static float DistanceSqrd(this GameObject a, GameObject b) => DistanceSqrd(a, b);
        #endregion

        #region InDistance
        /// <summary>
        /// Check if two Vector3s are within 'dist' of each other
        /// </summary>
        /// <param name="dist">[inclusive]</param>
        public static bool InDistance(this Vector3 a, Vector3 b, float dist) => InDistance(a, b, dist);
        /// <summary>
        /// Check if two Transforms are within 'dist' of each other
        /// </summary>
        /// <param name="dist">[inclusive]</param>
        public static bool InDistance(this Transform a, Transform b, float dist) => InDistance(a, b, dist);
        /// <summary>
        /// Check if to GameObjects are within 'dist' of each other
        /// </summary>
        /// <param name="dist">[inclusive]</param>
        public static bool InDistance(this GameObject a, GameObject b, float dist) => InDistance(a, b, dist);
        #endregion

        #region OutDistance
        /// <summary>
        /// Check if two Vector3s are further than 'dist' from each other
        /// </summary>
        /// <param name="dist">[exclusive]</param>
        public static bool OutDistance(this Vector3 a, Vector3 b, float dist) => OutDistance(a, b, dist);
        /// <summary>
        /// Check if two Transforms are further than 'dist' from each other
        /// </summary>
        /// <param name="dist">[exclusive]</param>
        public static bool OutDistance(this Transform a, Transform b, float dist) => OutDistance(a, b, dist);
        /// <summary>
        /// Check if two GameObjects are further than 'dist' from each other
        /// </summary>
        /// <param name="dist">[exclusive]</param>
        public static bool OutDistance(this GameObject a, GameObject b, float dist) => OutDistance(a, b, dist);
        #endregion
    }
}