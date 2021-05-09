using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Utility.Helper;

namespace Utility.ObjectPool
{
    [System.Serializable, InlineProperty, HideLabel]
    public struct PoolPrefab
    {
        [Required, OnValueChanged(nameof(RecalculateKey))]
        public GameObject prefab;

        [ReadOnly, SerializeField] public string key;

        private void RecalculateKey(GameObject prefab)
        {
            if (prefab == null)
            {
                key = "N/A";
                return;
            }

            UniqueID _id = UtilGO.ExtractComponent<UniqueID>(prefab);

            key = _id.UniqueIdentifier;
        }
    }

    [System.Serializable]
    public class ManagedPool
    {
        #region Prefab Settings
        [Tooltip("The prefab to create a pool of, will use the prefab name as a key")]
        [TabGroup("Prefab")]
        [SerializeField] private PoolPrefab m_poolPrefab;
        #endregion

        #region Size Settings
        [Tooltip("The initial maximum size of the pool")]
        [TabGroup("Size")]
        [SerializeField] [Min(1)] private int m_startSize;

        [Space]

        [Tooltip("Whether the pool can be resized at runtime")]
        [TabGroup("Size")]
        [SerializeField] private bool m_canResize;

        [Tooltip("The percentage to resize by, value of 10 will turn a pool of 10 -> 11, increased based on original size")]
        [TabGroup("Size"), ShowIf(nameof(m_canResize))]
        [SerializeField] [Min(0)] private float m_resizePercent;
        
        [Space()]

        [TabGroup("Size"), ShowIf(nameof(m_canResize)), ShowInInspector, ReadOnly]
        private int m_resizeCounter = 0;

        [TabGroup("Size")]
        [PropertyTooltip("How many objects are currently in the pool")]
        [ShowInInspector] private int CurrentPoolCount => m_pool.Count;
        #endregion

        public string Key => m_poolPrefab.key;

        private Queue<GameObject> m_pool = new Queue<GameObject>();

        private ObjectPool m_poolOwner;
        private GameObject m_poolContainer;

        public static string GetKey(GameObject prefab)
        {
            if (prefab == null) return "N/A";

            if (prefab.TryGetComponent<UniqueID>(out UniqueID _id))
            {
                return _id.UniqueIdentifier;
            }

            return "N/A";
        }

        public bool Initialise(ObjectPool poolOwner)
        {
            if (m_poolPrefab.prefab == null)
            {
                Debug.LogError($"There was no prefab to initialise the pool with", poolOwner);
                return false;
            }

            m_poolOwner = poolOwner;

            m_poolContainer = new GameObject(m_poolPrefab.prefab.name + " - Pool");
            m_poolContainer.transform.parent = m_poolOwner.transform;

            Populate(poolOwner, m_startSize);

            return true;
        }

        public void AddPrefab(GameObject prefab)
        {
            if (prefab == null) return;

            if (GetKey(prefab) != Key) return; // Not apart of this pool

            prefab.SetActive(false);
            m_pool.Enqueue(prefab);

            if (prefab.TryGetComponent(out PoolObjectHandler _handler))
            {
                _handler.OnDespawned();
            }
        }

        public GameObject GetPrefab(Vector3 pos, Vector3 eulerRot)
        {
            if (!CanGetPrefab()) return null;

            GameObject _spawnedObject = PreparePrefab(pos);
            _spawnedObject.transform.eulerAngles = eulerRot;

            return _spawnedObject;
        }
        public GameObject GetPrefab(Vector3 pos, Quaternion rot)
        {
            if (!CanGetPrefab()) return null;

            GameObject _spawnedObject = PreparePrefab(pos);
            _spawnedObject.transform.rotation = rot;

            return _spawnedObject;
        }

        private GameObject PreparePrefab(Vector3 pos)
        {
            GameObject _preparedObject = m_pool.Dequeue();

            _preparedObject.transform.position = pos;

            if (_preparedObject.TryGetComponent(out PoolObjectHandler _handler))
            {
                _handler.OnSpawned();
            }

            _preparedObject.SetActive(true);

            return _preparedObject;
        }

        private bool CanGetPrefab()
        {
            Resize();
            return (m_pool.Count != 0);
        }

        private void Populate(ObjectPool poolOwner, int count)
        {
            if (m_poolPrefab.prefab == null) return;

            for (int _poolIndex = 0; _poolIndex < count; _poolIndex++)
            {
                GameObject _poolObject = GameObject.Instantiate(m_poolPrefab.prefab, m_poolContainer.transform); 

                _poolObject.name = m_poolPrefab.prefab.name + " - PoolObject";
                _poolObject.SetActive(false);

                m_pool.Enqueue(_poolObject);

                if (!_poolObject.TryGetComponent(out PoolObjectHandler _handler)) continue;

                _handler.Initialise(poolOwner);
            }
        }
        private void Resize()
        {
            if (!m_canResize || m_pool.Count > 0) return;

            int _resizeCount = Mathf.RoundToInt((float)m_startSize * (m_resizePercent / 100f));

            Populate(m_poolOwner, _resizeCount);
            m_resizeCounter++;
        }
    }
}