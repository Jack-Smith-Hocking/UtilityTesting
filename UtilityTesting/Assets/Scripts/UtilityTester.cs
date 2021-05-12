using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper.Utility;
using Sirenix.OdinInspector;

public class UtilityTester : MonoBehaviour
{
    [FoldoutGroup("GlobalEvent 1")]
    public GlobalEventListener m_globalEventListener = new GlobalEventListener();

    [Space]

    public int m_iterationCount = 0;

    [Space]

    [TabGroup("GetClosest")]
    public int m_getClosestListSize = 0;
    [TabGroup("GetClosest")]
    public bool m_testGetClosest = false;

    [TabGroup("MonoInvoker")]
    public GameObject m_monoInvokerToggleObject;
    [TabGroup("MonoInvoker")]
    public bool m_testMonoInvoker = false;

    public void TestGetClosest()
    {
        if (!m_testGetClosest) return;

        List<Vector3> _points = new List<Vector3>(m_getClosestListSize);
        Vector3 _point = transform.position;

        for (int _listSizeIndex = 0; _listSizeIndex < m_getClosestListSize; _listSizeIndex++)
        {
            _points.Add(Util.Rand.Vector());
        }

        for (int _index = 0; _index < m_iterationCount; _index++)
        {
            Util.Math.GetClosestPoint(_point, _points, out Vector3 _closestPoint);
        }
    }

    private void TestMonoInvoker()
    {
        if (!m_testMonoInvoker) return;

        this.Invoker().FrameRepeat(() => { m_monoInvokerToggleObject.ToggleActive(); }, 1f);
    }

    private void Awake()
    {
        TestMonoInvoker();
        KeyCodeEventMapper.Instance.AddMappedKey(KeyCode.None, KeyCode.Tab, () => { print("Pressed Tab"); });
        m_globalEventListener.ListenToEvent(() => { print("testing events"); });
    }

    // Update is called once per frame
    void Update()
    {
        TestGetClosest();
    }
}
