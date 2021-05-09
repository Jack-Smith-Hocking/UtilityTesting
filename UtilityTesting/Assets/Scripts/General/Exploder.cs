using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private  float m_explodeForce = 0;
    [SerializeField] private  float m_explodeRadius = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit _rayHit)) return;
            
        Vector3 _explosionPos = _rayHit.point;

        Collider[] _colliders = Physics.OverlapSphere(_explosionPos, m_explodeRadius);

        foreach (Collider _col in _colliders)
        {
            if (_col.TryGetComponent<Rigidbody>(out Rigidbody _rb))
            {
                _rb.AddExplosionForce(m_explodeForce, _explosionPos, m_explodeRadius);
            }
        }
    }
}
