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

    public class GlobalEventManager : SerializedMonoBehaviour
    {
        public static GlobalEventManager Instance => m_singleton.Instance;
        private static Singleton<GlobalEventManager> m_singleton = new Singleton<GlobalEventManager>(nameof(GlobalEventManager));

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

        public void CallEvent(string eventName)
        {
            if (!m_globalEvents.ContainsKey(eventName)) return;

            m_globalEvents[eventName].m_event?.Invoke();
        }
    }
}