using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace GDML.Physics
{
    [RequireComponent(typeof(Collider))]
    public abstract class PhysicsCallback<T> : Callback<T>
    {
        [SerializeField, TabGroup("Enter")]
        [Tooltip("Callback for when this object enters a trigger/collision")] private UnityEvent m_onEnterEvent = null;

        [SerializeField, TabGroup("Stay")]
        [Tooltip("Callback for when this object stays in a trigger/collision")] private UnityEvent m_onStayEvent = null;

        [SerializeField, TabGroup("Exit")]
        [Tooltip("Callback for when this object exits a trigger/collision")] private UnityEvent m_onExitEvent = null;

        public static string CallbackType_Enter => "ENTER";
        public static string CallbackType_Stay => "STAY";
        public static string CallbackType_Exit => "EXIT";

        protected override void InitialiseCallbackDictionary(ref Dictionary<string, Action<T>> initDictionary)
        {
            if (initDictionary == null) throw new ArgumentNullException(nameof(initDictionary));

            initDictionary = new Dictionary<string, Action<T>>()
            {
                { CallbackType_Enter, (T data) => { } },
                { CallbackType_Exit, (T data) => { } },
                { CallbackType_Stay, (T data) => { } }
            };
        }

        protected virtual void OnEnter(T info)
        {
            m_onEnterEvent?.Invoke();
            base.InvokeCallback(CallbackType_Enter, info);
        }
        protected virtual void OnStay(T info)
        {
            m_onStayEvent?.Invoke();
            base.InvokeCallback(CallbackType_Stay, info);
        }
        protected virtual void OnExit(T info)
        {
            m_onExitEvent?.Invoke();
            base.InvokeCallback(CallbackType_Exit, info);
        }
    }
}