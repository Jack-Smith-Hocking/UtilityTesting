using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Jack.Utility;
using System;

namespace Utility.Config
{
    public class MultiConfigLoader : SerializedMonoBehaviour, IConfigurable
    {
        [SerializeField] private KeyCode m_prevConfigKeyCode = KeyCode.DownArrow;
        [SerializeField] private KeyCode m_nextConfigKeyCode = KeyCode.UpArrow;
        
        [Space]

        [SerializeField] private List<GameObject> m_configs = new List<GameObject>();

        [ShowInInspector] public int ConfigCount => m_configs.Count;
        [ShowInInspector] public GameObject CurrentConfig => LoadConfig();

        [SerializeField, ReadOnly] private int m_curruntCount = 0;

        public event Action<GameObject> OnConfigChanged;

        public GameObject LoadConfig()
        {
            if (m_configs.IsEmpty()) return null;

            ValidateCurrentCount();

            return m_configs[m_curruntCount];
        }

        private void Update()
        {
            if (IsPressed(m_prevConfigKeyCode)) IterateConfigs(-1);
            if (IsPressed(m_nextConfigKeyCode)) IterateConfigs(1);
        }

        private bool IsPressed(KeyCode keyCode) => Input.GetKeyDown(keyCode);

        private void IterateConfigs(int it)
        {
            int _prevCount = m_curruntCount;

            m_curruntCount += it;
            ValidateCurrentCount();

            if (m_curruntCount == _prevCount) return; 

            OnConfigChanged?.Invoke(LoadConfig());
        }

        private void ValidateCurrentCount()
        {
            m_curruntCount = Mathf.Clamp(m_curruntCount, 0, ConfigCount - 1);
        }
    }
}