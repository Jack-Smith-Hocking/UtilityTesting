using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Utility.Helper
{
    [System.Serializable]
    public struct MappedKey
    {
        [TabGroup("Key")]
        public KeyCode m_modifierKeyCode;
        [TabGroup("Key")]
        public KeyCode m_primaryKeyCode;

        [TabGroup("Event")]
        public UnityEvent m_event;

        [TabGroup("Description"), TextArea()]
        public string m_description;

        public MappedKey(KeyCode mod, KeyCode primary, System.Action mapAction, string description = "")
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
        public static KeyCodeEventMapper Instance => m_singleton.Instance;
        [SerializeField] private static Singleton<KeyCodeEventMapper> m_singleton = new Singleton<KeyCodeEventMapper>(nameof(KeyCodeEventMapper));

        [SerializeField] private List<MappedKey> m_mappedKeys = new List<MappedKey>();

        private void Awake()
        {
            m_singleton.SetInstance(this);

            FunctionUpdater.CreateUpdater(() =>
            {
                foreach (MappedKey _mappedKey in m_mappedKeys)
                {
                    if (_mappedKey.IsDown()) _mappedKey.m_event?.Invoke();
                }
            }, "MappedKeyCodes", true, UpdateType.NORMAL);
        }

        public void AddMappedKey(MappedKey mappedKey)
        {
            m_mappedKeys.Add(mappedKey);
        }
        public void AddMappedKey(KeyCode mod, KeyCode primary, System.Action mapAction, string description = "")
        {
            m_mappedKeys.Add(new MappedKey(mod, primary, mapAction, description));
        }
    }
}