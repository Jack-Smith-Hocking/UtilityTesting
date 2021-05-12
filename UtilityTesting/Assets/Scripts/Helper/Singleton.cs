using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utility.Helper
{
    public class Singleton<T> where T : MonoBehaviour
    {
        public T Instance => GetInstance();

        [ReadOnly, SerializeField] private T m_instance;

        private string m_className;

        public Singleton(string className) => m_className = className;

        /// <summary>
        /// Get (or create) an instance of this singleton type
        /// </summary>
        /// <returns>Singleton</returns>
        public T GetInstance()
        {
            if (CheckIfInstanceExists() == false)
            {
                CreateNewInstance();
            }

            return m_instance;
        }
        /// <summary>
        /// Set the instance of this singleton
        /// </summary>
        /// <param name="instance">Instance to manage</param>
        /// <param name="overrideCurrent">If there is a current singleton instance, overwrite it</param>
        /// <param name="destroyCurrent">If there is a current singleton instance, destroy it and attached GameObject</param>
        public void SetInstance(T instance, bool overrideCurrent = false, bool destroyCurrent = false)
        {
            if (m_instance.IsNotNull())
            {
                if (overrideCurrent == false) return;

                if (destroyCurrent) GameObject.Destroy(m_instance.gameObject);
            }

            m_instance = instance;
        }

        private bool CheckIfInstanceExists()
        {
            if (m_instance.IsNotNull()) return true;

            m_instance = GameObject.FindObjectOfType<T>();

            return m_instance.IsNotNull();
        }
        private void CreateNewInstance()
        {
            GameObject _global = GameObject.Find("_Global");

            if (_global.IsNull()) _global = new GameObject("_Global");

            GameObject _instanceObject = new GameObject($"_{m_className}");
            _instanceObject.transform.parent = _global.transform;

            m_instance = _instanceObject.AddComponent<T>();
        }
    }
}