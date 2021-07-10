using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Jack.Utility
{
    public static partial class Util
    {
        /// <summary>
        /// Utility class for GameObjects & Transforms
        /// </summary>
        public static class GOUtil
        {
            /// <summary>
            /// Get component from a GameObject, if there is none then one will be added
            /// </summary>
            /// <param name="obj">GameObject to extract component from</param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static T ExtractComponent<T>(GameObject obj) where T : Component
            {
                if (obj.IsNull()) return null;

                if (!obj.TryGetComponent<T>(out T _returnComp)) _returnComp = obj.AddComponent<T>();

                return _returnComp;
            }

            /// <summary>
            /// Get first child transform that matches 'childName', return null if none found
            /// </summary>
            /// <param name="childName">Name of child to find</param>
            /// <param name="root">The transform to check the children of</param>
            /// <returns></returns>
            public static Transform GetChild(Transform root, string childName)
            {
                for (int _childIndex = 0; _childIndex < root.childCount; _childIndex++)
                {
                    Transform _parent = root.GetChild(_childIndex);

                    if (_parent.name == childName)
                    {
                        return _parent;
                    }
                }

                return null;
            }
        }
    }

    public static partial class GO_Ext
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

        public static void SetParent(this GameObject obj, Transform parent) => obj.transform.parent = parent;
        public static Transform GetParent(this GameObject obj) => obj.transform.parent;

        public static bool InLayerMask(this GameObject obj, int mask) => obj.layer == (obj.layer | (1 << mask));
        public static bool InLayerMask(this GameObject obj, LayerMask mask) => InLayerMask(obj, mask.value);

        public static void ToggleActive(this GameObject obj) => obj.SetActive(!obj.activeInHierarchy);

        /// <summary>
        /// Get component from a GameObject, if there is none then one will be added
        /// </summary>
        /// <param name="obj">GameObject to extract component from</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ExtractComponent<T>(this GameObject obj) where T : Component
        {
            if (obj.IsNull()) return null;

            if (!obj.TryGetComponent<T>(out T _returnComp)) _returnComp = obj.AddComponent<T>();

            return _returnComp;
        }
    }
}