using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper.Utility;
using Helper.Updater;

public class SimpleCamera2DMover : MonoBehaviour
{
    public float m_moveSpeed;

    private Vector3 m_moveDir;

    private Camera m_mainCam;

    private void Start()
    {
        m_mainCam = Camera.main;

        KeyCodeEventMapper.Instance.AddMappedKey(KeyCode.None, KeyCode.UpArrow, KeyState.HELD, () => { m_moveDir += Vector3.up; }, "SimpleCamera2DMover -> Update the camera");
        KeyCodeEventMapper.Instance.AddMappedKey(KeyCode.None, KeyCode.DownArrow, KeyState.HELD, () => { m_moveDir += Vector3.down; }, "SimpleCamera2DMover -> Update the camera");

        KeyCodeEventMapper.Instance.AddMappedKey(KeyCode.None, KeyCode.LeftArrow, KeyState.HELD, () => { m_moveDir += Vector3.left; }, "SimpleCamera2DMover -> Update the camera");
        KeyCodeEventMapper.Instance.AddMappedKey(KeyCode.None, KeyCode.RightArrow, KeyState.HELD, () => { m_moveDir += Vector3.right; }, "SimpleCamera2DMover -> Update the camera");

        FunctionUpdater.CreateUpdater(() => { m_moveDir.z += Input.mouseScrollDelta.y; }, "SimpleCamera2dMover - Zoom", true, UpdateCycle.NORMAL);
    }

    private void LateUpdate()
    {
        Move(m_moveDir);     
    }

    private void Move(Vector3 dir)
    {
        m_mainCam.transform.position += m_moveDir.normalized * m_moveSpeed * Time.deltaTime;

        m_moveDir = Vector3.zero;
    }
}
