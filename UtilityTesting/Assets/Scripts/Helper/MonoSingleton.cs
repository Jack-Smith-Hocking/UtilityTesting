using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jack.Utility
{
    public class Singleton<T> where T : new()
    {
        public T Instance => GetInstance();
        private T m_instance;

        public Singleton() { }
        public Singleton(T instance)
        {
            if (instance.IsNull()) Debug.LogWarning($"Trying to initialise a Singleton<{typeof(T)}> with a null instance");
            m_instance = instance;
        }

        public T GetInstance()
        {
            if (m_instance.IsNull()) m_instance = new T();

            return m_instance;
        }
    }

    public class MonoSingleton<T> where T : MonoBehaviour
    {
        public T Instance => GetInstance();

        [ReadOnly, SerializeField] private T m_instance;

        private string m_className;
        private bool m_enforceSingleton;

        public MonoSingleton(string className, bool enforceSingleton = false)
        {
            m_className = className;
            m_enforceSingleton = enforceSingleton;
        }

        /// <summary>
        /// Get (or create) an instance of this singleton type
        /// </summary>
        /// <returns>Singleton</returns>
        public T GetInstance()
        {
            if (DoesInstanceExist() == false)
            {
                CreateNewInstance();
            }

            return m_instance;
        }
        /// <summary>
        /// Set the instance of this singleton
        /// </summary>
        /// <param name="instance">Instance to manage</param>
        public void SetInstance(T instance)
        {
            if (instance.IsNull()) return;

            if (m_instance.IsNotNull() && m_enforceSingleton)
            {
                Debug.LogWarning($"There can only be one instance of type {m_className} in the scene!") ;
                Debug.LogWarning($"To enforce singleton pattern, destroyed GameObject '{instance.gameObject.name}' with singleton '{m_className}'");

                GameObject.Destroy(instance.gameObject);

                return;
            }

            m_instance = instance;
        }

        private bool DoesInstanceExist()
        {
            if (m_instance.IsNotNull()) return true;

            m_instance = GameObject.FindObjectOfType<T>();

            return m_instance.IsNotNull();
        }

        private void CreateNewInstance()
        {
            GameObject _global = GameObject.Find("_Global");
            _global = _global ?? new GameObject("_Global"); // Assign if null

            GameObject _instanceObject = new GameObject($"_{m_className}");
            _instanceObject.transform.parent = _global.transform;

            m_instance = _instanceObject.AddComponent<T>();
        }
    }
}