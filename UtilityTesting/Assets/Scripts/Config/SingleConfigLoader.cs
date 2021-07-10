using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Utility.Config
{
    public class SingleConfigLoader : SerializedMonoBehaviour, IConfigurable
    {
        [SerializeField] private GameObject m_configObject;

        public event Action<GameObject> OnConfigChanged;

        public GameObject LoadConfig() => m_configObject;

        private void Start()
        {
            OnConfigChanged?.Invoke(LoadConfig());
        }
    }
}