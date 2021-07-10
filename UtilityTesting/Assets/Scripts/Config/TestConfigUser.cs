using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Utility.Config
{
    public class TestConfigUser : SerializedMonoBehaviour
    {
        [SerializeField] private IConfigurable m_configFetcher;

        public void Awake()
        {
            m_configFetcher.OnConfigChanged += (go) => Debug.LogError(go.name);
        }
    }
}