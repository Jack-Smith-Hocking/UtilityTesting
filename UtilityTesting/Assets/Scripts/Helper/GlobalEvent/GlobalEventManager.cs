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

        [Tooltip("Dictionary of global events")]
        [SerializeField] private Dictionary<string, GlobalEventData> m_globalEvents = new Dictionary<string, GlobalEventData>();

        public List<string> ListOfKeys => m_globalEvents.Keys.ToList();

        private void Awake() => m_singleton.SetInstance(this);

        /// <summary>
        /// Get the description of the specified global event
        /// </summary>
        public string GetEventDescription(string eventName)
        {
            bool _foundEvent = m_globalEvents.TryGetValue(eventName, out GlobalEventData _eventData);

            return _foundEvent ? _eventData.m_description : "N/A";
        }

        /// <summary>
        /// Listen to a global event, whenever that event is called the 'listener' action will be invoked
        /// </summary>
        /// <param name="eventName">Name of the event to listen to</param>
        /// <param name="listener">The action to invoke when the global event is triggered</param>
        /// <param name="addIfRequired">If the event doesn't exist already, add it</param>
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

        /// <summary>
        /// Add an event 
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="listener">Initial listener to the event</param>
        /// <param name="description">Description of the event</param>
        public void AddEvent(string eventName, System.Action listener, string description = "")
        {
            GlobalEventData _data = new GlobalEventData(listener, description);

            bool _valueHasBeenSet = m_globalEvents.TrySetValue(eventName, _data, false);

            if (_valueHasBeenSet) return;

            Debug.LogWarning($"Event of name '{eventName}' was already found in the global events, event was not added");
        }

        /// <summary>
        /// Call a global event
        /// </summary>
        /// <param name="eventName">Name of the event to call</param>
        public void CallEvent(string eventName)
        {
            if (!m_globalEvents.ContainsKey(eventName)) return;

            m_globalEvents[eventName].m_event?.Invoke();
        }
    }
}