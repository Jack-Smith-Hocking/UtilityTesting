using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.ObjectPool
{
    public interface IPooledObject
    {
        void OnSpawned();
        void OnDespawned();
    }
}