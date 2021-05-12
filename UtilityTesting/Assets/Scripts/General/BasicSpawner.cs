using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper.ObjectPool;
using Sirenix.OdinInspector;

public class BasicSpawner : MonoBehaviour
{
    [Min(0)] public float m_spawnDelay = 0;

    [Space]
    
    [BoxGroup("Pool")]
    public PoolPrefab m_poolPrefab;

    [BoxGroup("Pool"), InlineEditor]
    public ObjectPool m_pool;

    private float m_currentTime = 0;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time >= (m_currentTime + m_spawnDelay))
        {
            m_currentTime = Time.time + m_spawnDelay;

            m_pool.GetFromPool(m_poolPrefab.prefab, transform.position, transform.rotation);
        }
    }
}
