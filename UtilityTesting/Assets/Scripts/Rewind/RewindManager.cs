using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jack.Utility;

namespace Jack.Rewind
{
    public class RewindManager : MonoBehaviour
    {
        public static RewindManager Instance { get; private set; } = null;

        [SerializeField] private int m_maxRecordedSteps = 100;

        private bool m_isRewinding = false;

        private readonly List<ManagedRewindObject> m_rewindableObjects = new List<ManagedRewindObject>();

        public void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) { m_isRewinding = true; }
            else if (Input.GetKeyUp(KeyCode.Q)) { m_isRewinding = false; }
        }

        private void FixedUpdate()
        {
            if (!m_isRewinding)
            {
                foreach (ManagedRewindObject _rewindable in m_rewindableObjects) { _rewindable.AddTimeStep(); }

                return;
            }

            int _allStoppedRewind = 0;
            foreach (ManagedRewindObject _rewindable in m_rewindableObjects)
            {
                _rewindable.RewindStep();

                _allStoppedRewind += _rewindable.CanRewind.ToInt();
            }

            m_isRewinding = m_isRewinding && (_allStoppedRewind != 0);
        }

        public void AddRewindable(RewindObject rewindableObject)
        {
            if (rewindableObject == null) return;

            m_rewindableObjects.Add(new ManagedRewindObject(rewindableObject, m_maxRecordedSteps));
        }
    }
}