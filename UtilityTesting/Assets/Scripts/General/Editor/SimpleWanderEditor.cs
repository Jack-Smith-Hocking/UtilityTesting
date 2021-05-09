using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleWander))]
public class SimpleWanderEditor : Editor
{
    private void OnSceneGUI()
    {
        SimpleWander _target = target as SimpleWander;

        Handles.DrawWireDisc(_target.transform.position, Vector3.up, _target.m_maxWander);

        if (!Application.isPlaying) return;

        Handles.DrawLine(_target.transform.position, _target.m_target);

        Handles.color = Color.blue;

        Handles.DrawSolidDisc(_target.m_target, Vector3.up, 0.5f);
    }
}
