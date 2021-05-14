using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Helper.ObjectPool
{
    public interface IPooledObject
    {
        void OnSpawned();
        void OnDespawned();
    }

    public abstract class PoolBehaviour : MonoBehaviour, IPooledObject
    {
        public abstract void OnDespawned();
        public abstract void OnSpawned();

        protected void Dispose()
        {
            ObjectPool.Instance.Dispose(gameObject);
        }
    }

    [System.Serializable, InlineProperty, HideLabel]
    public class PoolFetcher
    {
        [OnInspectorGUI(nameof(UpdatePrefab))]
        [PropertyRange(0, nameof(GetMaxIndex))]
        [SerializeField] private int m_prefabIndex = 0;

        [SerializeField, ReadOnly, HideLabel] private ManagedPool m_pool;

        public string GetKey()
        {
            return m_pool.Key;
        }

        private void UpdatePrefab(int index)
        {
            m_pool = ObjectPool.Instance.GetPool(m_prefabIndex);
        }
        private int GetMaxIndex()
        {
            return ObjectPool.Instance.PoolCount - 1;
        }
    }
}