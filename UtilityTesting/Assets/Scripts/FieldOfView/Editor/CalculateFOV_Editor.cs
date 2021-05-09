using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace Utility.FOV
{
    [CustomEditor(typeof(CalculateFOV))]
    public class CalculateFOV_Editor : OdinEditor
    {
        private void OnSceneGUI()
        {
            CalculateFOV _fov = target as CalculateFOV;

            Handles.color = Color.white;

            Vector3 _viewAngleA = _fov.DirFromAngle(-_fov.ViewAngle / 2, false);
            Vector3 _viewAngleB = _fov.DirFromAngle(_fov.ViewAngle / 2, false);

            Handles.DrawWireArc(_fov.transform.position, Vector3.up, _fov.transform.forward, 360, _fov.ViewRadius); // Full Circle
            //Handles.DrawWireArc(_fov.transform.position, Vector3.up, _viewAngleA, _fov.ViewAngle, _fov.ViewRadius); // Pie

            Handles.DrawLine(_fov.transform.position, _fov.transform.position + (_viewAngleA * _fov.ViewRadius));
            Handles.DrawLine(_fov.transform.position, _fov.transform.position + (_viewAngleB * _fov.ViewRadius));

            Handles.color = Color.red;
            foreach (Transform _target in _fov.VisibleTargets)
            {
                Handles.DrawLine(_fov.transform.position, _target.position);
            }
        }
    }
}