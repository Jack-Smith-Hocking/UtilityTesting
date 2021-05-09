using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Rewind
{
    public class RewindObject : MonoBehaviour
    {
        private Rigidbody m_rigidbody = null;
        private bool m_isRigidbodyNull;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_isRigidbodyNull = m_rigidbody != null;

            RewindManager.Instance.AddRewindable(this);
        }

        public void SetKinematicState(bool state)
        {
            if (m_isRigidbodyNull) m_rigidbody.isKinematic = state; 
        }
    }
}