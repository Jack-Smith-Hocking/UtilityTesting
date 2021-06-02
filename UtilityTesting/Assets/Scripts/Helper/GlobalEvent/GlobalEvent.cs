using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Jack.Utility
{
    [InlineProperty, System.Serializable, HideLabel]
    public class GlobalEventNameFetcher
    {
        [Tooltip("The EventManager to fetch events from")]
        [HideIf(nameof(m_useManagerInstance))]
        [SerializeField, Required] private GlobalEventManager m_globalEventManager;
        
        [Tooltip("Whether or not to use the global event manager")]
        [SerializeField] private bool m_useManagerInstance;

        [Space]

        [ValueDropdown(nameof(GetGlobalEventsOptions))]
        public string m_eventName;

        public GlobalEventManager Manager => m_useManagerInstance ? GlobalEventManager.Instance : m_globalEventManager;
        public bool HasManager => Manager.IsNotNull();

        private List<string> GetGlobalEventsOptions()
        {
            if (!HasManager)
            {
                m_eventName = "N/A";
                return new List<string>();
            }

            List<string> _options = Manager.ListOfKeys;

            if (!_options.Contains(m_eventName)) m_eventName = "N/A";

            return _options;
        }
    }

    [InlineProperty, HideLabel, System.Serializable]
    public class GlobalEventListener
    {
        [TabGroup("NameFetcher")]
        [Tooltip("Fetches the name of events to minimise mistakes")]
        [OnInspectorGUI(nameof(UpdateSelected))]
        [SerializeField] private GlobalEventNameFetcher m_nameFetcher = new GlobalEventNameFetcher();

        [TabGroup("Listener")]
        [Tooltip("The name of the event you want to add")]
        [InlineButton(nameof(AddEvent))]
        [SerializeField] private string m_addEventName;

        [TabGroup("Listener")]
        [Tooltip("The description of the currently selected event")]
        [ReadOnly, TextArea, HideLabel]
        [SerializeField] private string m_eventDescription;

        /// <summary>
        /// Listen to the event associated with this GlobalEventListener
        /// </summary>
        /// <param name="listener">Action to listen to global event</param>
        public void ListenToEvent(System.Action listener)
        {
            if (!m_nameFetcher.HasManager) return;

            m_nameFetcher.Manager.ListenToEvent(m_nameFetcher.m_eventName, listener);
        }

        private void UpdateSelected()
        {
            if (!m_nameFetcher.HasManager) return;

            m_eventDescription = m_nameFetcher.Manager.GetEventDescription(m_nameFetcher.m_eventName);
        }

        private void AddEvent()
        {
            if (!m_nameFetcher.HasManager) return;

            m_nameFetcher.Manager.AddEvent(m_addEventName, () => { }, $"Event added by a GlobalEventListener");
            m_nameFetcher.m_eventName = m_addEventName;
            m_addEventName = "";
        }
    }
}