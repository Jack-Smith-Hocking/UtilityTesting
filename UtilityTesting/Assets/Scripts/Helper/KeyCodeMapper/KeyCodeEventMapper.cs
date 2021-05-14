using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Helper.Utility
{
    [System.Serializable]
    public struct KeyCodeEventData
    {
        [TabGroup("Key")]
        public KeyCode m_modifierKeyCode;
        [TabGroup("Key")]
        public KeyCode m_primaryKeyCode;

        [TabGroup("Event")]
        public UnityEvent m_event;

        [TabGroup("Description"), TextArea()]
        public string m_description;

        public KeyCodeEventData(KeyCode mod, KeyCode primary, System.Action mapAction, string description = "")
        {
            m_modifierKeyCode = mod;
            m_primaryKeyCode = primary;

            m_event = new UnityEvent();
            m_event.AddListener(() => { mapAction?.Invoke(); });

            m_description = description;
        }

        public bool IsDown()
        {
            bool _mod = m_modifierKeyCode == KeyCode.None || Input.GetKey(m_modifierKeyCode);

            return _mod && Input.GetKeyDown(m_primaryKeyCode);
        }
    }

    public class KeyCodeEventMapper : SerializedMonoBehaviour
    {
        public static KeyCodeEventMapper Instance => s_singleton.Instance;
        private static Singleton<KeyCodeEventMapper> s_singleton = new Singleton<KeyCodeEventMapper>(nameof(KeyCodeEventMapper));

        [FoldoutGroup("GlobalEventFetcher")]
        [Tooltip("Fetches the name of global events")]
        [SerializeField] private GlobalEventNameFetcher m_nameFetcher;
        
        [Space]

        [Tooltip("Dictionary of mapped keys")]
        [SerializeField] private List<KeyCodeEventData> m_mappedKeys = new List<KeyCodeEventData>();

        private void Awake()
        {
            s_singleton.SetInstance(this);

            FunctionUpdater.CreateUpdater(() =>
            {
                foreach (KeyCodeEventData _mappedKey in m_mappedKeys)
                {
                    if (_mappedKey.IsDown()) _mappedKey.m_event?.Invoke();
                }
            }, "MappedKeyCodes", true, UpdateType.NORMAL);
        }

        public void AddMappedKey(KeyCodeEventData mappedKey)
        {
            m_mappedKeys.Add(mappedKey);
        }
        public void AddMappedKey(KeyCode mod, KeyCode primary, System.Action mapAction, string description = "")
        {
            m_mappedKeys.Add(new KeyCodeEventData(mod, primary, mapAction, description));
        }
    }
}