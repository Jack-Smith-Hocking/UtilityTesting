using System;
using System.Collections;
using System.Collections.Generic;
using Jack.ObjectPool;
using UnityEngine;

using Random = UnityEngine.Random;

public class RandomForce : PoolBehaviour
{
    public float m_lifeSpan = 5;
    public float m_upForce = 0;
    public float m_sideForce = 0;
    
    private Rigidbody m_rigidBody;
    private float m_life = 0;

    private 
    
    // Start is called before the first frame update
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Time.time > m_life)
        {
            Dispose();
        }
    }

    public override void OnSpawned()
    {
        m_life = Time.time + m_lifeSpan;
        
        float _xForce = Random.Range(-m_sideForce, m_sideForce);
        float _yForce = Random.Range(m_upForce / 2f, m_upForce);
        float _zForce = Random.Range(-m_sideForce, m_sideForce);

        Vector3 _force = new Vector3(_xForce, _yForce, _zForce);

        m_rigidBody.velocity = _force;
    }

    public override void OnDespawned()
    {
        m_rigidBody.velocity = Vector3.zero;
    }
}
