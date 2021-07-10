using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace Jack.Utility
{
    public static class MathUtil
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
        public static Vector3 Direction(Component p1, Component p2, bool norm = true) => Direction(p1.ToPoint(), p2.ToPoint(), norm);

        public static Vector3 Direction(GameObject p1, GameObject p2, bool norm = true) => Direction(p1.ToPoint(), p2.ToPoint(), norm);

        /// <summary>
        /// Returns direction from start to end
        /// </summary>
        public static Vector3 Direction(Vector3 start, Vector3 end, bool norm = true) => norm ? (end - start).normalized : (end - start);
        #endregion
    }

    public static class Math_Ext
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
    }
}