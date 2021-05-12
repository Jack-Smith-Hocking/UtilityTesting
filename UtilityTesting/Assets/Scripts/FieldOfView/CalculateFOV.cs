using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper.Utility;
using Sirenix.OdinInspector;
using System;

namespace Helper.FOV
{
    public class CalculateFOV : MonoBehaviour
    {
        #region View Settings
        [TabGroup("View"), Tooltip("The maximum viewable radius")]
        [SerializeField] private float m_viewRadius;
        [TabGroup("View"), Tooltip("The arc angle that is calculated as visible in front of the GameObject")]
        [SerializeField] private float m_viewAngle;
        [TabGroup("View"), Tooltip("The time intervals for calculating visible targets")]
        [SerializeField, Min(0)] private float m_viewTargetInterval = 0.2f;
        #endregion

        #region Mask Settings 
        [TabGroup("Mask"), Tooltip("GameObjects on this layer will be added to a list of visible targets if in the view arc")]
        [SerializeField] private LayerMask m_targetMask;
        [TabGroup("Mask"), Tooltip("Will be the layer that is used to determine objects that block view")]
        [SerializeField] private LayerMask m_obstacleMask;
        #endregion

        public List<Transform> VisibleTargets { get; private set; } = new List<Transform>();

        public float ViewRadius => m_viewRadius;
        public float ViewAngle => m_viewAngle;

        public int TargetMask => m_targetMask.value;
        public int ObstacleMask => m_obstacleMask.value;

        private Collider[] m_targetsInView;

        private int m_testLoopInt = 0;

        private void Start()
        {
            StartCoroutine(nameof(FindTargetWithDelay), m_viewTargetInterval);
        }

        /// <summary>
        /// Delay the calculation of finding visible targets, runs in a continuous loop
        /// </summary>
        /// <param name="delay">The time delay before each calculation</param>
        /// <returns></returns>
        private IEnumerator FindTargetWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);

                FindVisibleTargets();
            }
        }
        /// <summary>
        /// Calculate the visible targets
        /// </summary>
        public void FindVisibleTargets()
        {
            // Preliminary cast for finding all the targets in the radius
            m_targetsInView = Physics.OverlapSphere(transform.position, ViewRadius, m_targetMask.value);

            VisibleTargets.Clear();
            VisibleTargets.Capacity = m_targetsInView.Length;

            for (int _targetIndex = 0; _targetIndex < m_targetsInView.Length; _targetIndex++)
            {
                Transform _targetTransform = m_targetsInView[_targetIndex].transform;
                Vector3 _targetDir = _targetTransform.position - transform.position;
                float _dist = _targetDir.magnitude;

                _targetDir.Normalize();

                float _viewAngle = Vector3.Angle(transform.forward, _targetDir);

                if (_viewAngle >= ViewAngle / 2) continue; // If the object is outside of the viewing angle, it is not visible
                if (Physics.Raycast(transform.position, _targetDir, _dist, m_obstacleMask.value)) continue; // If there is something blocking line of sight then it is not visible

                VisibleTargets.Add(_targetTransform);
            }
        }


        /// <summary>
        /// Get a direction using the input angle, based on the transfrom
        /// </summary>
        /// <param name="angleInDegrees">Angle to get a direction from, in degrees</param>
        /// <param name="angleIsGlobal">Whether the given angle is global or local</param>
        /// <returns></returns>
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y; // Convert to global angle

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}