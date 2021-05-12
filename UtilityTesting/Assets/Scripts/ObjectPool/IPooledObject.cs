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
}