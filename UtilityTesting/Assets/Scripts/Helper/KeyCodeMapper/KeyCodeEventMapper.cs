using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Utility.Helper
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
        [FoldoutGroup("GlobalEventFetcher")]
        [SerializeField] private GlobalEventNameFetcher m_nameFetcher;

        public static KeyCodeEventMapper Instance => m_singleton.Instance;
        [SerializeField] private static Singleton<KeyCodeEventMapper> m_singleton = new Singleton<KeyCodeEventMapper>(nameof(KeyCodeEventMapper));

        [SerializeField] private List<KeyCodeEventData> m_mappedKeys = new List<KeyCodeEventData>();

        private void Awake()
        {
            m_singleton.SetInstance(this);

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