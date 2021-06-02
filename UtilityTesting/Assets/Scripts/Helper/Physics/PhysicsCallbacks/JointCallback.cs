using System;
using System.Collections;
using System.Collections.Generic;
using Jack.Utility;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Jack.Utility.Physics
{
    [RequireComponent(typeof(Joint))]
    public class JointCallback : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Callback for when this joint breaks")] private UnityEvent m_onBreakEvent = null;

        [SerializeField]
        [Tooltip("The Joint that is being managed, if no Joint is input will get the first found on on this gameObject")] private Joint m_managedJoint = null;

        public event System.Action<float> OnJointBreakEvent;

        protected void Awake() => ValidateJoint();


        /// <summary>
        /// Add a RigidBody to connect the Joint to
        /// </summary>
        /// <param name="connectedBody">The body to connect to</param>
        /// <param name="triggerJointBreakCallback">Whether or not to trigger the OnJointBreak callback (with value of 0) of the old body</param>
        public void SetConnectedBody(Rigidbody connectedBody, bool triggerJointBreakCallback = false)
        {
            if (triggerJointBreakCallback && m_managedJoint.connectedBody != null)
            {
                OnJointBreak(0);
            }

            m_managedJoint.connectedBody = connectedBody;
        }
        
        /// <summary>
        /// Break the connection of this Joint
        /// </summary>
        /// <param name="triggerJointBreakCallback">Whether or not to trigger the OnJointBreak callback (with value of 0)</param>
        public void BreakConnectedBody(bool triggerJointBreakCallback = false)
        {
            if (triggerJointBreakCallback)
            {
                OnJointBreak(0);
            }

            m_managedJoint.connectedBody = null;
        }
        
        /// <summary>
        /// Make sure there is a valid Joint being managed
        /// </summary>
        private void ValidateJoint()
        {
            if (m_managedJoint != null) return;

            gameObject.TryGetComponent<Joint>(out m_managedJoint);
        }
        
        private void OnJointBreak(float breakForce)
        {
            m_onBreakEvent.Invoke();
            OnJointBreakEvent?.Invoke(breakForce);
        }
    }
}