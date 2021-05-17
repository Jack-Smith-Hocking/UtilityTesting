using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Helper.Updater;

namespace Helper.Utility
{
    public enum KeyState
    { 
        DOWN,
        HELD
    }

    [System.Serializable]
    public struct KeyCodeEventData
    {
        [TabGroup("Key")]
        public KeyCode m_modifierKeyCode;
        [TabGroup("Key")]
        public KeyCode m_primaryKeyCode;

        [TabGroup("Key")]
        public KeyState m_keyState;

        [TabGroup("Event")]
        public UnityEvent OnActiveEvent;
        public System.Action OnActive;

        [TabGroup("Description"), TextArea()]
        public string m_description;

        public KeyCodeEventData(KeyCode mod, KeyCode primary, KeyState keyState, System.Action mapAction, string description = "")
        {
            m_modifierKeyCode = mod;
            m_primaryKeyCode = primary;

            m_keyState = keyState;

            OnActiveEvent = new UnityEvent();
            OnActive = mapAction;

            m_description = description;
        }

        public void ActivateEvents()
        {
            OnActiveEvent?.Invoke();
            OnActive?.Invoke();
        }

        public bool IsStateActive() => IsStateActive(m_modifierKeyCode, m_primaryKeyCode, m_keyState);
        public static bool IsStateActive(KeyCode mod, KeyCode primary, KeyState keyState)
        {
            switch (keyState)
            {
                case KeyState.DOWN: return IsDown(mod, primary);
                case KeyState.HELD: return IsHeld(mod, primary);

                default: return false;
            }
        }

        public static bool IsModDown(KeyCode mod) => mod == KeyCode.None || Input.GetKey(mod);

        public bool IsDown() => IsDown(m_modifierKeyCode, m_primaryKeyCode);
        public static bool IsDown(KeyCode mod, KeyCode primary) => IsModDown(mod) && Input.GetKeyDown(primary);

        public bool IsHeld() => IsHeld(m_modifierKeyCode, m_primaryKeyCode);
        public static bool IsHeld(KeyCode mod, KeyCode primary) => IsModDown(mod) && Input.GetKey(primary);
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
                    CheckKeyCodeEvent(_mappedKey);
                }
            }, "MappedKeyCodes", true, UpdateCycle.NORMAL);
        }

        public void AddMappedKey(KeyCodeEventData mappedKey)
        {
            m_mappedKeys.Add(mappedKey);
        }
        public void AddMappedKey(KeyCode mod, KeyCode primary, KeyState keyState, System.Action mapAction, string description = "")
        {
            m_mappedKeys.Add(new KeyCodeEventData(mod, primary, keyState, mapAction, description));
        }

        private void CheckKeyCodeEvent(KeyCodeEventData eventData)
        {
            bool _modIsDown = false;
            KeyCode _validMod = KeyCode.None;

            // Loop through all the aliases for 
            KeyCodeAliasManager.PerformOnAliases(eventData.m_modifierKeyCode, (KeyCode alias) =>
            {
                _modIsDown = KeyCodeEventData.IsModDown(alias);
                _validMod = alias;

                return _modIsDown;
            }, true);

            if (_modIsDown == false) return;

            bool _primIsDown = false;

            KeyCodeAliasManager.PerformOnAliases(eventData.m_primaryKeyCode, (KeyCode alias) =>
            {
                _primIsDown = KeyCodeEventData.IsStateActive(_validMod, alias, eventData.m_keyState);

                return _primIsDown;
            }, true);

            if (_primIsDown == false) return;

            eventData.ActivateEvents();
        }
    }
}