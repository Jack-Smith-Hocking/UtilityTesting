using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Utility.Helper;
using Sirenix.OdinInspector;

namespace Utility.ObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        [Tooltip("Future Feature -> To allow runtime pool generation")]
        [SerializeField] private bool m_allowDefaultSettings = false;

        [Tooltip("Future Feature -> To allow runtime pool generation")]
        [HideIfGroup("@" + nameof(m_allowDefaultSettings) + " == false")]
        [SerializeField] private ManagedPool m_defaultPool;

        [Space]

        [Tooltip("List of pools and associated data")]
        [SerializeField] private List<ManagedPool> m_poolData;

        private readonly Dictionary<string, ManagedPool> m_poolDict = new Dictionary<string, ManagedPool>();

        public event Action OnObjectCreated;
        public event Action OnObjectReturned;

        private void Awake()
        {
            InitialiseAllPools();
        }

        /// <summary>
        /// Populate all of the pools that have valid prefabs
        /// </summary>
        private void InitialiseAllPools()
        {
            // Attempt to initialise all the pools and add them to the dictionary if succeeded
            foreach (var poolData in m_poolData)
            {
                if (!poolData.Initialise(this)) continue;

                m_poolDict[poolData.Key] = poolData; 
            }
        }

        /// <summary>
        /// Get a GameObject from a pool if there is any available
        /// </summary>
        /// <param name="prefab">The prefab to get, uses the prefab name</param>
        /// <param name="position">Position to set the acquired GameObject to</param>
        /// <param name="rotation">Rotation to set the acquired GameObject to</param>
        /// <returns></returns>
        public GameObject GetFromPool([NotNull] GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null) return null;

            string _key = ManagedPool.GetKey(prefab);

            return GetFromPool(_key, position, rotation);
        }
        
        public GameObject GetFromPool(string key, Vector3 position, Quaternion rotation)
        {
            if (!m_poolDict.ContainsKey(key)) return null;

            OnObjectCreated?.Invoke();

            return m_poolDict[key].GetPrefab(position, rotation);
        }

        /// <summary>
        /// Return a GameObject to it's associated pool
        /// </summary>
        /// <param name="prefab">The prefab to return</param>
        public void ReturnToPool([NotNull] GameObject prefab)
        {
            if (prefab == null) return;

            string _key = ManagedPool.GetKey(prefab);

            if (!m_poolDict.ContainsKey(_key)) return;

            m_poolDict[_key].AddPrefab(prefab);

            OnObjectReturned?.Invoke();
        }
    }
}