using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Jack.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObject/KeyCodeAliasSettings", fileName = "New KeyCodeAliasSettings")]
    public class KeyCodeAliasManager : SerializedScriptableObject
    {
        public static KeyCodeAliasManager Instance => s_instance;
        private static KeyCodeAliasManager s_instance = null;

        [SerializeField] private Dictionary<KeyCode, List<KeyCode>> m_aliases = new Dictionary<KeyCode, List<KeyCode>>();

        private static bool s_warningMessageSent = false;

        /// <summary>
        /// Perform an action on all the aliases that were found for a keycode
        /// </summary>
        /// <param name="keyCode">The KeyCode to look for aliases of</param>
        /// <param name="loopFunction">The Func that will be run on all aliases</param>
        /// <param name="performOnOriginal">Whether the loopFunction will be invoked on the original KeyCode</param>
        public static void PerformOnAliases(KeyCode keyCode, System.Func<KeyCode, bool> loopFunction, bool performOnOriginal = true)
        {
            bool _validAliasFound = false;

            if (loopFunction.IsNull()) return;
            if (performOnOriginal) _validAliasFound = loopFunction.Invoke(keyCode);

            if (s_instance.IsNull())
            {
                if (s_warningMessageSent == false)
                {
                    Debug.LogWarning("No KeyCodeAliasManager is currently set up!");
                    s_warningMessageSent = true;
                }
                return;
            }

            if (s_instance.m_aliases.DoesNotContainKey(keyCode)) return;

            foreach (KeyCode _alias in s_instance.m_aliases[keyCode])
            {
                if (_validAliasFound) return;

                _validAliasFound = loopFunction.Invoke(_alias);
            }
        }

        private void OnEnable()
        {
            if (s_instance.IsNotNull() && this.Equals(s_instance) == false)
            {
                Debug.LogWarning("There can only be one KeyCodeAliasManager asset in the project at a time");

                DestroyImmediate(this, true);
                return;
            }

            s_instance = this;
        }
    }
}