using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Utility.Helper
{
    public static class UtilGO
    {
        /// <summary>
        /// Get component from a GameObject, if there is none then one will be added
        /// </summary>
        /// <param name="obj">GameObject to extract component from</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ExtractComponent<T>([NotNull] GameObject obj) where T : Component
        {
            if (obj == null) return null;

            if (!obj.TryGetComponent<T>(out T _returnComp)) _returnComp = obj.AddComponent<T>();

            return _returnComp;
        }

        /// <summary>
        /// Get first child transform that matches 'childName', return null if none found
        /// </summary>
        /// <param name="childName">Name of child to find</param>
        /// <param name="root">The transform to check the children of</param>
        /// <returns></returns>
        public static Transform GetChild(string childName, [NotNull] Transform root)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));

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
