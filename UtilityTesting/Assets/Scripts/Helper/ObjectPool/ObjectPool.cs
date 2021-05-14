using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Helper.Utility;
using Sirenix.OdinInspector;

namespace Helper.ObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        [Tooltip("List of pools and associated data")]
        [SerializeField] private List<ManagedPool> m_poolData;

        public static ObjectPool Instance => s_singleton.Instance;
        private static Singleton<ObjectPool> s_singleton = new Singleton<ObjectPool>(nameof(ObjectPool), true);

        private Dictionary<string, ManagedPool> m_poolDict = new Dictionary<string, ManagedPool>();

        public event Action OnObjectCreated;
        public event Action OnObjectReturned;

        private void Awake()
        {
            s_singleton.SetInstance(this);

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
        public GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab.IsNull()) return null;

            string _key = PoolPrefab.GetKey(prefab);

            return Create(_key, position, rotation);
        }
        
        public GameObject Create(string key, Vector3 position, Quaternion rotation)
        {
            if (m_poolDict.DoesNotContainKey(key)) return null;

            OnObjectCreated?.Invoke();

            GameObject _obj = m_poolDict[key].Get(position, rotation);
            UpdatePooledObjects(_obj, (IPooledObject _poolObj) => _poolObj.OnSpawned());

            return _obj;
        }

        /// <summary>
        /// Return a GameObject to it's associated pool
        /// </summary>
        /// <param name="prefab">The prefab to return</param>
        public void Dispose(GameObject prefab)
        {
            if (prefab.IsNull()) return;

            string _key = PoolPrefab.GetKey(prefab);

            if (m_poolDict.DoesNotContainKey(_key)) return;

            UpdatePooledObjects(prefab, (IPooledObject _poolObj) => _poolObj.OnDespawned());

            m_poolDict[_key].Add(prefab);

            OnObjectReturned?.Invoke();
        }

        private void UpdatePooledObjects(GameObject obj, System.Action<IPooledObject> action)
        {
            if (obj.IsNull()) return;

            IPooledObject[] _pooledObjects = obj.GetComponents<IPooledObject>();
            foreach (var _poolObject in _pooledObjects)
            {
                action?.Invoke(_poolObject);
            }
        }
    }
}