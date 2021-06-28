using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

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

            public static float DotToRad(float dotProduct) => Mathf.Acos(dotProduct);
            public static float DotToDegree(float dotProduct) => Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

            public static bool InLayerMask(int layer, int mask) => layer == (layer | (1 << mask));
            public static bool InLayerMask(int layer, LayerMask mask) => InLayerMask(layer, mask.value);

            /// <summary>
            /// Return true if value is >= min and <= max
            /// </summary>
            /// <param name="value"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static bool InRange(int value, int min, int max) => value >= min && value <= max;
            /// <summary>
            /// Return true if value is >= min and <= max
            /// </summary>
            /// <param name="value"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static bool InRange(float value, float min, float max) => value >= min && value <= max;

            #region DirectionTo
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

            #region GetClosest
            public static Vector3 GetClosest(Vector3 anchor, Vector3 pointOne, Vector3 pointTwo) => DistanceSqrd(pointOne, anchor) < DistanceSqrd(pointTwo, anchor) ? pointOne : pointTwo;
            public static Transform GetClosest(Transform anchor, Transform pointOne, Transform pointTwo) => DistanceSqrd(pointOne, anchor) < DistanceSqrd(pointTwo, anchor) ? pointOne : pointTwo;
            public static GameObject GetClosest(GameObject anchor, GameObject pointOne, GameObject pointTwo) => DistanceSqrd(pointOne, anchor) < DistanceSqrd(pointTwo, anchor) ? pointOne : pointTwo;

            public static bool GetClosestPoint(Vector3 point, List<Vector3> points, out Vector3 closest)
            {
                if (points.IsEmpty()) { closest = Vector3.zero; return false; }

                points.Sort((a, b) => { return DistanceSqrd(a, point).CompareTo(DistanceSqrd(b, point)); });
                closest = points[0];

                return true;
            }
            public static bool GetClosestPoint(Transform point, List<Transform> points, out Transform closest)
            {
                if (points.IsEmpty()) { closest = null; return false; }

                points.Sort((a, b) => { return DistanceSqrd(a, point).CompareTo(DistanceSqrd(b, point)); });
                closest = points[0];

                return true;
            }
            public static bool GetClosestPoint(GameObject point, List<GameObject> points, out GameObject closest)
            {
                if (points.IsEmpty()) { closest = null; return false; }

                points.Sort((a, b) => { return DistanceSqrd(a, point).CompareTo(DistanceSqrd(b, point)); });
                closest = points[0];

                return true;
            }
            public static bool GetClosestPoint(Collider point, List<Collider> points, out Collider closest) => GetClosestPoint(point.gameObject, points, out closest);

            public static bool GetClosestPoint(GameObject point, List<Collider> points, out Collider closest)
            {
                if (points.Count == 0) { closest = null; return false; }

                points.Sort((a, b) => { return DistanceSqrd(a.transform, point.transform).CompareTo(DistanceSqrd(b.transform, point.transform)); });
                closest = points[0];

                return true;
            }
            #endregion

            #region Distance Squared
            public static float DistanceSqrd(Vector2 a, Vector2 b) => (a - b).sqrMagnitude;
            public static float DistanceSqrd(Vector3 a, Vector3 b) => (a - b).sqrMagnitude;
            public static float DistanceSqrd(Transform a, Transform b) => (a.position - b.position).sqrMagnitude;
            public static float DistanceSqrd(GameObject a, GameObject b) => (a.transform.position - b.transform.position).sqrMagnitude;
            #endregion

            #region InDistance
            public static bool InDistance(Vector2 a, Vector2 b, float dist) => (a - b).sqrMagnitude <= (dist * dist);
            public static bool InDistance(Vector3 a, Vector3 b, float dist) => (a - b).sqrMagnitude <= (dist * dist);
            public static bool InDistance(Transform a, Transform b, float dist) => (a.position - b.position).sqrMagnitude <= (dist * dist);
            public static bool InDistance(GameObject a, GameObject b, float dist) => (a.transform.position - b.transform.position).sqrMagnitude <= (dist * dist);
            #endregion

            #region OutDistance
            public static bool OutDistance(Vector2 a, Vector2 b, float dist) => (a - b).sqrMagnitude > (dist * dist);
            public static bool OutDistance(Vector3 a, Vector3 b, float dist) => (a - b).sqrMagnitude > (dist * dist);
            public static bool OutDistance(Transform a, Transform b, float dist) => (a.position - b.position).sqrMagnitude > (dist * dist);
            public static bool OutDistance(GameObject a, GameObject b, float dist) => (a.transform.position - b.transform.position).sqrMagnitude > (dist * dist);
            #endregion
        }
    }

    public static class ExtMath
    {
        public static int Sqrd(this int num) => (num * num);
        public static float Sqrd(this float num) => (num * num);

        public static int ToInt(this bool boolean) => boolean ? 1 : 0;

        public static bool InLayerMask(this int layer, int mask) => layer == (layer | (1 << mask));
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
        public static Vector3 DirectionTo(this Vector3 start, Vector3 end, bool norm = true) => norm ? (end - start).normalized : (end - start);
        public static Vector3 DirectionTo(this Transform start, Transform end, bool norm = true) => DirectionTo(start.position, end.position, norm);
        public static Vector3 DirectionTo(this GameObject start, GameObject end, bool norm = true) => DirectionTo(start.transform.position, end.transform.position, norm);
        #endregion

        #region Distance Squared
        public static float DistanceSqrd(this Vector2 a, Vector2 b) => (a - b).sqrMagnitude;
        public static float DistanceSqrd(this Vector3 a, Vector3 b) => (a - b).sqrMagnitude;
        public static float DistanceSqrd(this Transform a, Transform b) => (a.position - b.position).sqrMagnitude;
        public static float DistanceSqrd(this GameObject a, GameObject b) => (a.transform.position - b.transform.position).sqrMagnitude;
        #endregion

        #region InDistance
        public static bool InDistance(this Vector2 a, Vector2 b, float dist) => (a - b).sqrMagnitude <= (dist * dist);
        public static bool InDistance(this Vector3 a, Vector3 b, float dist) => (a - b).sqrMagnitude <= (dist * dist);
        public static bool InDistance(this Transform a, Transform b, float dist) => (a.position - b.position).sqrMagnitude <= (dist * dist);
        public static bool InDistance(this GameObject a, GameObject b, float dist) => (a.transform.position - b.transform.position).sqrMagnitude <= (dist * dist);
        #endregion

        #region OutDistance
        public static bool OutDistance(this Vector2 a, Vector2 b, float dist) => (a - b).sqrMagnitude > (dist * dist);
        public static bool OutDistance(this Vector3 a, Vector3 b, float dist) => (a - b).sqrMagnitude > (dist * dist);
        public static bool OutDistance(this Transform a, Transform b, float dist) => (a.position - b.position).sqrMagnitude > (dist * dist);
        public static bool OutDistance(this GameObject a, GameObject b, float dist) => (a.transform.position - b.transform.position).sqrMagnitude > (dist * dist);
        #endregion
    }
}