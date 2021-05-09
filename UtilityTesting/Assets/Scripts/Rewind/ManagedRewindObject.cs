using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Rewind
{
    public readonly struct PointInTime
    {
        public readonly Vector3 position;
        public readonly Vector3 eulerRotation;

        public PointInTime(Vector3 pos, Vector3 eulerRot)
        {
            position = pos;
            eulerRotation = eulerRot;
        }
    }

    public class ManagedRewindObject
    {
        private readonly RewindObject m_managedRewindable;
        private readonly LinkedList<PointInTime> m_pointsInTime;
        private readonly int m_maxRecordTime;

        public bool CanRewind => (m_pointsInTime.Count > 0);
        public int RecordedSteps => (m_pointsInTime.Count);

        public ManagedRewindObject(RewindObject managedRewindable, int maxRecordedSteps)
        {
            m_pointsInTime = new LinkedList<PointInTime>();
            m_managedRewindable = managedRewindable;
            m_maxRecordTime = maxRecordedSteps;
        }

        public void AddTimeStep()
        {
            if (!m_managedRewindable.gameObject.activeInHierarchy)
            {
                m_pointsInTime.Clear();

                return;
            }

            m_managedRewindable.SetKinematicState(false);

            Transform _transform = m_managedRewindable.transform;

            m_pointsInTime.AddLast(new PointInTime(_transform.position, _transform.eulerAngles));

            if (m_pointsInTime.Count > m_maxRecordTime) m_pointsInTime.RemoveFirst();
        }

        public void RewindStep()
        {
            if (!m_managedRewindable.gameObject.activeInHierarchy) return;

            int _count = m_pointsInTime.Count;

            if (_count == 0) return;

            m_managedRewindable.SetKinematicState(true);
            UpdatePointInTime(m_pointsInTime.Last.Value);

            m_pointsInTime.RemoveLast();
        }

        private void UpdatePointInTime(PointInTime pointInTime)
        {
            Transform _transform = m_managedRewindable.transform;

            _transform.position = pointInTime.position;
            _transform.eulerAngles = pointInTime.eulerRotation;
        }
    }
}