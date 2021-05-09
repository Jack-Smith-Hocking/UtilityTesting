using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDML.Physics
{
    public abstract class Callback<T> : MonoBehaviour
    {
        private Dictionary<string, Action<T>> m_actionCallbacks = new Dictionary<string, Action<T>>();

        /// <summary>
        /// Called during awake to initialise the dictionary of callbacks
        /// </summary>
        protected abstract void InitialiseCallbackDictionary(ref Dictionary<string, Action<T>> initDictionary);

        protected virtual void Awake()
        {
            InitialiseCallbackDictionary(ref m_actionCallbacks);
        }

        /// <summary>
        /// Invoke a callback from the dictonary if it exists
        /// </summary>
        /// <param name="callbackType">The type of callback to invoke</param>
        /// <param name="data">The data to parse the invoked callback</param>
        protected void InvokeCallback(string callbackType, T data)
        {
            if (m_actionCallbacks.ContainsKey(callbackType))
            {
                m_actionCallbacks[callbackType]?.Invoke(data);
            }
        }

        /// <summary>
        /// Add a listener to a callback
        /// </summary>
        /// <param name="callbackType">The callback type to listen to</param>
        /// <param name="callbackAction">The action to perform when callback is triggered</param>
        public void AddListener(string callbackType, Action<T> callbackAction)
        {
            if (m_actionCallbacks.ContainsKey(callbackType))
            {
                m_actionCallbacks[callbackType] += callbackAction;
            }
        }
        /// <summary>
        /// Remove a listener from a callback
        /// </summary>
        /// <param name="callbackType">The type of callback to remove from</param>
        /// <param name="callbackAction">The action to remove from the callback</param>
        public void RemoveListener(string callbackType, Action<T> callbackAction)
        {
            if (m_actionCallbacks.ContainsKey(callbackType))
            {
                m_actionCallbacks[callbackType] -= callbackAction;
            }
        }
    }
}