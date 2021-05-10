using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Utility.Helper
{
    /// <summary>
    /// Holds the data for an event trigger
    /// </summary>
    [System.Serializable]
    public struct DelayedEventData
    {
        // Details
        [TabGroup("Details")]
        public string m_eventName;

        [TabGroup("Details"), Tooltip("How long after the previous event should be waited")]
        public float m_eventDelay;
        //

        // Description
        [TabGroup("Description"), TextArea(4, 4)]
        public string m_eventDescription;
        //

        // Events
        [TabGroup("Events"), Tooltip("Event that gets triggered by something")]
        public UnityEvent m_onEventTriggered;
        //
    }

    /// <summary>
    /// Triggers a set of events one after another
    /// </summary>
    public class DelayedEvents : MonoBehaviour
    {
        // Trigger
        [TabGroup("Trigger"), InlineButton(nameof(TriggerEvents))]
        [SerializeField] private bool m_triggerOnlyOnce = false;

        [Tooltip("How long after triggering to be able to re-trigger")]
        [TabGroup("Trigger"), ShowIf("@" + nameof(m_triggerOnlyOnce) + " == false")]
        [SerializeField, Min(0)] private float m_triggerCooldown = 0;
        //

        // Private
        [TabGroup("Debugging"), ReadOnly, ShowInInspector]
        private float m_lastTriggerTime = 0;
        [TabGroup("Debugging"), ReadOnly, ShowInInspector]
        private bool m_hasTriggered = false;
        //

        [SerializeField] private List<DelayedEventData> m_events = new List<DelayedEventData>();

        /// <summary>
        /// Starts triggering the events
        /// </summary>
        private void StartEventChain(Action<DelayedEventData> action)
        {
            for (int i = 0; i < m_events.Count; i++)
            {
                StartCoroutine(FireEvent(action, m_events[i]));
            }
        }

        /// <summary>
        /// Fire an event (Enter, Stay or Exit) after a delay
        /// </summary>
        /// <param name="action">Action to perform on an event</param>
        /// <param name="ed">The EventData to act upon</param>
        /// <returns></returns>
        IEnumerator FireEvent(Action<DelayedEventData> action, DelayedEventData ed)
        {
            yield return new WaitForSeconds(ed.m_eventDelay);

            action?.Invoke(ed);
        }

        public void TriggerEvents()
        {
            if (m_triggerOnlyOnce && m_hasTriggered) return;

            if (Time.time > m_lastTriggerTime + m_triggerCooldown)
            {
                m_lastTriggerTime = Time.time;
                m_hasTriggered = true;

                StartEventChain((DelayedEventData d) => { d.m_onEventTriggered.Invoke(); });
            }
        }
    }
}