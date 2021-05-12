using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Helper.ObjectPool
{
    [DisallowMultipleComponent]
    public class PoolObjectHandler : MonoBehaviour
    {
        private ObjectPool m_owner;
        private List<PooledObject> m_poolObjects = new List<PooledObject>();

        public void Initialise(ObjectPool owner)
        {
            m_owner = owner;

            PooledObject[] _poolObjects = GetComponents<PooledObject>();
            for (int _objectIndex = 0; _objectIndex < _poolObjects.Length; _objectIndex++)
            {
                _poolObjects[_objectIndex].SetHandler(this);
            }

            m_poolObjects.AddRange(_poolObjects);
        }

        public void OnSpawned()
        {
            foreach (var _poolObject in m_poolObjects)
            {
                _poolObject.OnSpawned();
            }
        }

        public void OnDespawned()
        {
            foreach (var _poolObject in m_poolObjects)
            {
                _poolObject.OnDespawned();
            }
        }

        public void Despawn()
        {
            if (m_owner == null) return;

            m_owner.ReturnToPool(gameObject);
        }
    }
}