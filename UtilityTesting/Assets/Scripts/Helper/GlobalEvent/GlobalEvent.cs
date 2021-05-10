using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Utility.Helper
{
    [InlineProperty, System.Serializable, HideLabel]
    public class GlobalEventNameFetcher
    {
        [HideIf(nameof(m_useManagerInstance))]
        [SerializeField, Required] protected GlobalEventManager m_globalEventManager;

        [SerializeField, Required] protected bool m_useManagerInstance;

        [Space]

        [ValueDropdown(nameof(GetGlobalEventsOptions))]
        public string m_eventName;

        public GlobalEventManager Manager => m_useManagerInstance ? GlobalEventManager.Instance : m_globalEventManager;

        public List<string> GetGlobalEventsOptions()
        {
            if (UtilGen.IsNUll(Manager))
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
        [SerializeField] private GlobalEventNameFetcher m_nameFetcher = new GlobalEventNameFetcher();

        [TabGroup("Listener")]
        [InlineButton(nameof(AddEvent))]
        [SerializeField] private string m_addEventName;

        [TabGroup("Listener")]
        [ReadOnly, TextArea, HideLabel]
        [SerializeField] private string m_eventDescription;

        public void ListenToEvent(System.Action listener)
        {
            if (UtilGen.IsNUll(m_nameFetcher.Manager)) return;

            m_nameFetcher.Manager.ListenToEvent(m_nameFetcher.m_eventName, listener);
        }

        private void UpdateSelected(string newEventName)
        {
            if (UtilGen.IsNUll(m_nameFetcher.Manager)) return;

            m_eventDescription = m_nameFetcher.Manager.GetEventDescription(newEventName);

            m_nameFetcher.GetGlobalEventsOptions();
        }

        private void AddEvent()
        {
            if (UtilGen.IsNUll(m_nameFetcher.Manager)) return;

            m_nameFetcher.Manager.AddEvent(m_addEventName, () => { }, $"Event added by a GlobalEventListener");
            m_nameFetcher.m_eventName = m_addEventName;
            m_addEventName = "";
        }
    }
}