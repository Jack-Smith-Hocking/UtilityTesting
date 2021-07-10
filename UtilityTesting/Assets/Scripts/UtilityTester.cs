using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jack.Utility;
using Sirenix.OdinInspector;

public class UtilityTester : MonoBehaviour
{
    public int m_iterationCount;
    public bool m_useCustom;

    [Button("TestLogging")]
    public void TestLogging(int count, bool useCustom)
    {
        for (int i = 0; i < count; i++)
        {
            if (useCustom)
            {
                DebugLogger.Log($"TestLog: {i}", 0, this);
            }
            else
            {
                Debug.Log($"TestLog: {i}", this);
            }
        }
    }

    public void Update()
    {
        TestLogging(m_iterationCount, m_useCustom);    
    }

    [Button("Log")]
    void TestLog(int priority)
    {
        DebugLogger.Log("Test log!!", priority);
    }

    [Button("LogWarning")]
    void TestLogWarning(int priority)
    {
        DebugLogger.LogWarning("Test log warning!!", priority);
    }

    [Button("LogError")]
    void TestLogError(int priority)
    {
        DebugLogger.LogError("Test log error!!", priority);
    }
}
