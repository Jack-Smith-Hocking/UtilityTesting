using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utility.Helper
{
    [System.Serializable]
    public struct GlobalEventData
    {
        [TabGroup("Event")]
        public UnityEvent m_event;

        [TabGroup("Description"), TextArea()]
        public string m_description;

        public GlobalEventData(System.Action listener, string description)
        {
            m_event = new UnityEvent();
            m_event.AddListener(() => { listener?.Invoke(); });

            m_description = description;
        }
    }

    [InlineProperty, HideLabel]
    public class GlobalEventListener
    {
        [FoldoutGroup("Event Listener")]
        [SerializeField, Required] private GlobalEventManager m_eventManger;

        [Space]

        [FoldoutGroup("Event Listener")]
        [InlineButton(nameof(AddEvent))]
        [SerializeField] private string m_addEventName;

        [Space(8)]

        [FoldoutGroup("Event Listener")]
        [ValueDropdown(nameof(GetGLobalEventsOptions)), OnInspectorGUI(nameof(UpdateSelected))]
        [SerializeField] private string m_eventName;

        [FoldoutGroup("Event Listener")]
        [ReadOnly, TextArea, HideLabel]
        [SerializeField] private string m_eventDescription;

        private void UpdateSelected(string newEventName)
        {
            if (m_eventManger == null) return;

            m_eventDescription = m_eventManger.GetEventDescription(newEventName);

            GetGLobalEventsOptions();
        }

        private List<string> GetGLobalEventsOptions()
        {
            if (m_eventManger == null)
            {
                m_eventName = "N/A";
                return new List<string>();
            }

            List<string> _options = m_eventManger.ListOfKeys;

            if (!_options.Contains(m_eventName)) m_eventName = "N/A";

            return _options;
        }

        private void AddEvent()
        {
            if (m_eventManger == null) return;

            m_eventManger.AddEvent(m_addEventName, () => { }, $"Event added by a GlobalEventListener");
            m_eventName = m_addEventName;
        }
    }

    public class GlobalEventManager : SerializedMonoBehaviour
    {
        public static GlobalEventManager Instance => m_singleton.Instance;
        private static Singleton<GlobalEventManager> m_singleton = new Singleton<GlobalEventManager>(nameof(GlobalEventManager));

        public GlobalEventListener m_eventListener;

        [SerializeField] private Dictionary<string, GlobalEventData> m_globalEvents = new Dictionary<string, GlobalEventData>();

        public List<string> ListOfKeys => m_globalEvents.Keys.ToList();

        private void Awake() => m_singleton.SetInstance(this);

        public string GetEventDescription(string eventName)
        {
            bool _foundEvent = m_globalEvents.TryGetValue(eventName, out GlobalEventData _eventData);

            return _foundEvent ? _eventData.m_description : "N/A";
        }

        public void ListenToEvent(string eventName, System.Action listener, bool addIfRequired = false)
        {
            if (Instance.m_globalEvents.TryGetValue(eventName, out GlobalEventData _data))
            {
                _data.m_event.AddListener(() => { listener?.Invoke(); });

                return;
            }

            if (!addIfRequired) return;

            AddEvent(eventName, listener, $"Added the '{eventName}' event due to not finding it in the global events dictionary");
        }

        public void AddEvent(string eventName, System.Action listener, string description = "")
        {
            GlobalEventData _data = new GlobalEventData(listener, description);

            bool _valueHasBeenSet = m_globalEvents.TrySetValue(eventName, _data, false);

            if (_valueHasBeenSet) return;

            Debug.LogWarning($"Event of name '{eventName}' was already found in the global events, event was not added");
        }
    }
}