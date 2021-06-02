using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Jack.Utility.Physics
{
    [RequireComponent(typeof(Collider))]
    public abstract class PhysicsCallback<T> : MonoBehaviour
    {
        [SerializeField, TabGroup("Enter")]
        [Tooltip("Callback for when this object enters a trigger/collision")] private UnityEvent m_onEnterEvent = null;

        [SerializeField, TabGroup("Stay")]
        [Tooltip("Callback for when this object stays in a trigger/collision")] private UnityEvent m_onStayEvent = null;

        [SerializeField, TabGroup("Exit")]
        [Tooltip("Callback for when this object exits a trigger/collision")] private UnityEvent m_onExitEvent = null;

        public event System.Action<T> OnEnterEvent;
        public event System.Action<T> OnStayEvent;
        public event System.Action<T> OnExitEvent;


        protected virtual void OnEnter(T info)
        {
            m_onEnterEvent?.Invoke();
            OnEnterEvent?.Invoke(info);
        }
        protected virtual void OnStay(T info)
        {
            m_onStayEvent?.Invoke();
            OnStayEvent?.Invoke(info);
        }
        protected virtual void OnExit(T info)
        {
            m_onExitEvent?.Invoke();
            OnExitEvent?.Invoke(info);
        }
    }
}