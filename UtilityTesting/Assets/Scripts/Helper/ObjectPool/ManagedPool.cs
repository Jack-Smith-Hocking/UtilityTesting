using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Helper.Utility;

namespace Helper.ObjectPool
{
    [System.Serializable, InlineProperty, HideLabel]
    public struct PoolPrefab
    {
        [Required, OnValueChanged(nameof(RecalculateKey))]
        [SerializeField] private GameObject m_prefab;

        [ReadOnly, SerializeField] private string m_key;

        public GameObject Prefab => m_prefab;
        public string PrefabName => m_prefab.IsNull() ? "N/A" : m_prefab.name;
        public string Key => m_key;

        private void RecalculateKey(GameObject prefab) => m_key = GetKey(prefab);

        public static string GetKey(GameObject prefab)
        {
            if (prefab.IsNull()) return "N/A";

            return prefab.ExtractComponent<UniqueID>().UniqueIdentifier;
        }
    }

    [System.Serializable, InlineProperty]
    public class ManagedPool
    {
        #region Prefab Settings
        [Tooltip("The prefab to create a pool of, will use the UniqueID as a key")]
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

        [PropertySpace]

        [TabGroup("Size")]
        [PropertyTooltip("How many objects are currently in the pool")]
        [ShowInInspector] private int CurrentPoolCount => m_pool.Count;
        #endregion

        public string Key => m_poolPrefab.Key;

        private bool HasPrefab => m_poolPrefab.Prefab.IsNotNull();
        private bool m_hasInitialised = false;

        private Queue<GameObject> m_pool = new Queue<GameObject>();

        private GameObject m_poolContainer;

        private void Initialise()
        {
            if (m_hasInitialised) return;

            if (HasPrefab == false)
            {
                Debug.LogError($"There was no prefab to initialise the pool with");
                return;
            }

            m_poolContainer = new GameObject(m_poolPrefab.PrefabName + " - Pool");
            m_poolContainer.SetParent(ObjectPool.Instance.transform);

            Populate(m_startSize);

            m_hasInitialised = true;
        }

        public void Add(GameObject prefab)
        {
            if (prefab.IsNull()) return;
            if (PoolPrefab.GetKey(prefab) != Key) return; // Not apart of this pool

            prefab.SetActive(false);
            m_pool.Enqueue(prefab);
        }

        public GameObject Get(Vector3 pos, Vector3 eulerRot)
        {
            if (CanGet() == false) return null;

            return Prepare(pos, eulerRot);
        }
        public GameObject Get(Vector3 pos, Quaternion rot) => Get(pos, rot.eulerAngles);

        private GameObject Prepare(Vector3 pos, Vector3 euler)
        {
            GameObject _preparedObject = m_pool.Dequeue();
            _preparedObject.SetActive(true);

            _preparedObject.transform.position = pos;
            _preparedObject.transform.eulerAngles = euler;

            return _preparedObject;
        }

        private bool CanGet()
        {
            Initialise();
            Resize();

            return m_pool.Count > 0;
        }

        private void Create()
        {
            if (HasPrefab == false) return;

            string _prefabName = m_poolPrefab.PrefabName + " - PoolObject";

            GameObject _poolObject = GameObject.Instantiate(m_poolPrefab.Prefab, m_poolContainer.transform);
            Add(_poolObject);
        }

        private void Populate(int count)
        {
            if (HasPrefab == false) return;

            for (int _poolIndex = 0; _poolIndex < count; _poolIndex++)
            {
                Create();
            }
        }
        private void Resize()
        {
            if (!m_canResize || m_pool.Count > 0) return;

            int _resizeCount = Mathf.RoundToInt((float)m_startSize * (m_resizePercent / 100f));

            Populate(_resizeCount);
        }
    }
}