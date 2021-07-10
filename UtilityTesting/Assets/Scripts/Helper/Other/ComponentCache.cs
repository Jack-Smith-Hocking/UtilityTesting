using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jack.Utility;
using UnityEngine;

namespace Jack.Utility
{
    [DisallowMultipleComponent]
    public class ComponentCache
    {
        private Dictionary<System.Type, List<Component>> m_cachedDict = new Dictionary<System.Type, List<Component>>();

        /// <summary>
        /// List of the last requested component
        /// </summary>
        private List<Component> m_cachedList = new List<Component>();
        /// <summary>
        /// Last requested type
        /// </summary>
        private System.Type m_cachedType = null;

        /// <summary>
        /// GameObject to cache the components of
        /// </summary>
        private GameObject m_cachedObject = null;

        public ComponentCache(GameObject obj)
        {
            m_cachedObject = obj;

            Cache();
        }

        /// <summary>
        /// ReCache components on GameObject
        /// </summary>
        public void ReCache()
        {
            m_cachedDict.Clear();
            Cache();
        }

        /// <summary>
        /// Get the first component of the given type
        /// </summary>
        /// <typeparam name="TComp">Type of component to get</typeparam>
        /// <returns></returns>
        public TComp GetFirstComponent<TComp>() where TComp : Component
        {
            // Code for GetAllComponents is basically the same so reduced this
            // Function to just calculate the whole list and return the first item
            if (GetAllComponents(typeof(TComp)).Count == 0) return null;

            return m_cachedList[0] as TComp;
        }

        /// <summary>
        /// Get all the components of the given type
        /// </summary>
        /// <param name="checkType">The type to get all components of</param>
        /// <returns></returns>
        public List<Component> GetAllComponents(System.Type checkType)
        {
            if (IsCached(checkType)) return m_cachedList;
            if (m_cachedDict.DoesNotContainKey(checkType)) return new List<Component>();

            SetCache(checkType);

            return m_cachedList;
        }
        /// <summary>
        /// Get all the components of the given type
        /// </summary>
        /// <typeparam name="TComp">The component to get</typeparam>
        /// <returns></returns>
        public List<Component> GetAllComponents<TComp>() where TComp : Component { return GetAllComponents(typeof(TComp)); }

        private void SetCache(System.Type cachedType)
        {
            m_cachedList = m_cachedDict[cachedType];
            m_cachedType = cachedType;
        }

        private bool IsCached(System.Type checkType) => checkType == m_cachedType;

        /// <summary>
        /// Get all of the components on the cached GameObject and add to dictionary
        /// </summary>
        private void Cache()
        {
            List<Component> _components = new List<Component>(m_cachedObject.GetComponents<Component>());

            foreach (Component _comp in _components)
            {
                System.Type _compType = _comp.GetType();
                GenUtil.ValidateCollectionValue(_compType, m_cachedDict);

                m_cachedDict[_compType].Add(_comp);
            }
        }
    }

    public static partial class ExtGen
    {
        private static Dictionary<GameObject, ComponentCache> m_cachedTypes = new Dictionary<GameObject, ComponentCache>();

        public static ComponentCache Cache(this GameObject obj)
        {
            if (m_cachedTypes.ContainsKey(obj)) return m_cachedTypes[obj];

            m_cachedTypes[obj] = new ComponentCache(obj);

            return m_cachedTypes[obj];
        }
    }
}
