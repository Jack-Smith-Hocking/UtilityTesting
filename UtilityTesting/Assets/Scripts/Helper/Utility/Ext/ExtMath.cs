using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Helper.Utility
{
    public static class ExtMath
    {
        public static int Sqrd(this int num) => (num * num);
        public static float Sqrd(this float num) => (num * num);

        public static int ToInt(this bool boolean) => boolean ? 1 : 0;

        public static bool InLayerMask(this int layer, int mask) => layer == (layer | (1 << mask));
        public static bool InLayerMask(this int layer, LayerMask mask) => InLayerMask(layer, mask.value);

        public static Vector3 DirectionTo(this Vector3 start, Vector3 end) => (end - start).normalized;
        public static Vector3 DirectionTo(this Transform start, Transform end) => (end.position - start.position).normalized;
        public static Vector3 DirectionTo(this GameObject start, GameObject end) => (end.transform.position - start.transform.position).normalized;

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