using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper.Utility
{
    public static class ExtGO
    {
        /// <summary>
        /// Get the world position
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Vector3 GetPosition(this GameObject obj) => obj.transform.position;
        /// <summary>
        /// Set the world position
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pos"></param>
        public static void SetPosition(this GameObject obj, Vector3 pos) => obj.transform.position = pos;

        /// <summary>
        /// Get the world euler rotation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Vector3 GetEulerRotation(this GameObject obj) => obj.transform.eulerAngles;
        /// <summary>
        /// Set the world euler rotation
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eulerRot"></param>
        public static void SetEulerRotation(this GameObject obj, Vector3 eulerRot) => obj.transform.eulerAngles = eulerRot;

        public static bool InLayerMask(this GameObject obj, int mask) => obj.layer == (obj.layer | (1 << mask));
        public static bool InLayerMask(this GameObject obj, LayerMask mask) => InLayerMask(obj, mask.value);

        public static void ToggleActive(this GameObject obj) => obj.SetActive(!obj.activeInHierarchy);
    }
}