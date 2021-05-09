using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.ObjectPool
{
    public abstract class PooledObject : MonoBehaviour, IPooledObject
    {
        protected PoolObjectHandler m_handler;

        public void SetHandler(PoolObjectHandler handler) => m_handler = handler;

        protected void Despawn()
        {
            m_handler.Despawn();
        }

        public abstract void OnDespawned();
        public abstract void OnSpawned();
    }
}