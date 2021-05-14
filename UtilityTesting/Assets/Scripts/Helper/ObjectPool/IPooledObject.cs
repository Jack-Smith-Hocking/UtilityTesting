using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}