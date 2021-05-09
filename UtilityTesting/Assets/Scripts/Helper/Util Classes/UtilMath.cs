using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Utility.Helper
{
    public static class UtilMath
    {
        /// <summary>
        /// Rotate a vector by an angle given an axis
        /// </summary>
        /// <param name="angle">Angle (in degrees) to rotate by</param>
        /// <param name="axis">The axis to rotate around</param>
        /// <param name="dir">THe direction to rotate</param>
        /// <returns>A rotated vector</returns>
        public static Vector3 RotateBy(float angle, Vector3 axis, Vector3 dir) => Quaternion.AngleAxis(angle, axis.normalized) * dir.normalized;

        public static bool InLayerMask(int layer, int mask) => layer == (layer | (1 << mask));
        public static bool InLayerMask(int layer, LayerMask mask) => InLayerMask(layer, mask.value);

        public static Vector3 Direction(Vector3 start, Vector3 end) => (end - start).normalized;
        public static Vector3 Direction(Transform start, Transform end) => (end.position - start.position).normalized;
        public static Vector3 Direction(GameObject start, GameObject end) => (end.transform.position - start.transform.position).normalized;

        #region GetClosest
        public static bool GetClosestPoint(Vector3 point, List<Vector3> points, out Vector3 closest)
        {
            if (points.Count == 0) { closest = Vector3.zero; return false; }

            points.Sort((a, b) => { return DistanceSqrd(a, point).CompareTo(DistanceSqrd(b, point)); });
            closest = points[0];

            return true;
        }
        public static bool GetClosestPoint(Transform point, List<Transform> points, out Transform closest)
        {
            if (points.Count == 0) { closest = null; return false; }

            points.Sort((a, b) => { return DistanceSqrd(a, point).CompareTo(DistanceSqrd(b, point)); });
            closest = points[0];

            return true;
        }
        public static bool GetClosestPoint(GameObject point, List<GameObject> points, out GameObject closest)
        {
            if (points.Count == 0) { closest = null; return false; }

            points.Sort((a, b) => { return DistanceSqrd(a, point).CompareTo(DistanceSqrd(b, point)); });
            closest = points[0];

            return true;
        }
        public static bool GetClosestPoint(Collider point, List<Collider> points, out Collider closest)
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