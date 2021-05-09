using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Helper
{
    public static class ExtGO
    {
        public static Vector3 GetPosition(this GameObject obj) => obj.transform.position;
        public static void SetPosition(this GameObject obj, Vector3 pos) => obj.transform.position = pos;

        public static Vector3 GetEulerRotation(this GameObject obj) => obj.transform.eulerAngles;
        public static void SetEulerRotation(this GameObject obj, Vector3 eulerRot) => obj.transform.eulerAngles = eulerRot;

        public static bool InLayerMask(this GameObject obj, int mask) => obj.layer == (obj.layer | (1 << mask));
        public static bool InLayerMask(this GameObject obj, LayerMask mask) => InLayerMask(obj, mask.value);

        #region Distance
        public static float DistanceSqrd(this Transform a, Transform b) => (a.position - b.position).sqrMagnitude;
        public static float DistanceSqrd(this GameObject a, GameObject b) => (a.transform.position - b.transform.position).sqrMagnitude;

        public static bool InDistance(this Transform a, Transform b, float dist) => (a.position - b.position).sqrMagnitude <= (dist * dist);
        public static bool InDistance(this GameObject a, GameObject b, float dist) => (a.transform.position - b.transform.position).sqrMagnitude <= (dist * dist);

        public static bool OutDistance(this Transform a, Transform b, float dist) => (a.position - b.position).sqrMagnitude > (dist * dist);
        public static bool OutDistance(this GameObject a, GameObject b, float dist) => (a.transform.position - b.transform.position).sqrMagnitude > (dist * dist);
        #endregion
    }
}