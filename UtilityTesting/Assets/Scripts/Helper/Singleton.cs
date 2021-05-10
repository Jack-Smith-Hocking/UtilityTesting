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

        public Singleton(string className)
        {
            m_className = className;
        }

        public T GetInstance()
        {
            if (UtilGen.IsNotNull(m_instance)) return m_instance;

            if (!CheckIfInstanceExists())
            {
                CreateNewInstance();
            }

            return m_instance;
        }
        public void SetInstance(T instance, bool overrideCurrent = false, bool destroyCurrent = false)
        {
            if (UtilGen.IsNotNull(m_instance))
            {
                if (!overrideCurrent) return;

                if (destroyCurrent) GameObject.Destroy(m_instance.gameObject);
            }

            m_instance = instance;
        }

        private bool CheckIfInstanceExists()
        {
            GameObject _existingObject = GameObject.Find($"_{m_className}");
            
            if (UtilGen.IsNUll(_existingObject)) return false;
            
            _existingObject.TryGetComponent<T>(out m_instance);

            if (m_instance == null)
            {
                m_instance = _existingObject.AddComponent<T>();
            }

            return true;
        }
        private void CreateNewInstance()
        {
            GameObject _global = GameObject.Find("_Global");

            if (UtilGen.IsNUll(_global)) _global = new GameObject("_Global");

            GameObject _instanceObject = new GameObject($"_{m_className}");
            _instanceObject.transform.parent = _global.transform;

            m_instance = _instanceObject.AddComponent<T>();
        }
    }
}