using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Jack.Utility;

public class TypeCacheBehaviour : SerializedMonoBehaviour
{
    public ComponentCache m_typeCache;

    [BoxGroup("Debug")]
    public GameObject m_target;
    [BoxGroup("Debug")]
    public bool m_useRandType;
    [BoxGroup("Debug")]
    public bool m_useCache;
    [BoxGroup("Debug")]
    public bool m_cacheCache;
    [BoxGroup("Debug")]
    public int m_iterationCount;

    [BoxGroup("Type")]
    public System.Type m_currentType = null;
    [BoxGroup("Type")]
    public List<System.Type> m_validTypes = new List<System.Type>();

    // Start is called before the first frame update
    void Start()
    {
        List<Component> _components = new List<Component>(m_target.GetComponents<Component>());
        m_validTypes = new List<System.Type>(_components.Count);

        foreach (Component _comp in _components)
        {
            System.Type _compType = _comp.GetType();
            m_validTypes.Add(_compType);
        }

        m_target = m_target ?? gameObject;
        m_typeCache = m_target.Cache();
    }

    private void Update()
    {
        for (int _index = 0; _index < m_iterationCount; _index++)
        {
            m_currentType = m_useRandType ? m_validTypes.Rand() : m_validTypes[0];
            if (m_useCache)
            {
                if (m_cacheCache)
                {
                    m_typeCache.GetAllComponents(m_currentType);
                }
                else
                {
                    m_target.Cache().GetAllComponents(m_currentType);
                }
            }
            else
            {
                m_target.GetComponents(m_currentType);
            }
        }
    }
}
