using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Helper.Utility;

public class SimpleWander : MonoBehaviour
{
    public NavMeshAgent m_navAgent;
    [Space]
    [Min(0)] public float m_maxWander;
    [Space]
    public Vector3 m_target;

    private void Start()
    {
        UpdatePosition();
    }
    private void Update()
    {
        Vector3 _pos = transform.position;
        _pos.y = m_target.y;

        if (Util.Math.InDistance(_pos, m_target, m_navAgent.stoppingDistance) || m_navAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        m_target = Util.Rand.NavMeshPosition(transform.position, m_maxWander, 1000);

        m_navAgent.SetDestination(m_target);
    }
}
