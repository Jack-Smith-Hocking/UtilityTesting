using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jack.Updater;
using Jack.Utility;

public class SimpleController : MonoBehaviour
{
    public bool m_screenCentreBasedFacing = true;
    public float m_moveSpeed = 6;

    private Rigidbody m_rigidBody;
    private Camera m_viewCam;

    private Vector3 m_moveDir;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_viewCam = Camera.main;

        KeyCodeEventMapper.Instance.AddMappedKey(KeyCode.None, KeyCode.UpArrow, KeyState.HELD, () => { m_moveDir.z += 1; }, "SimpleCamera2DMover -> Update the camera");
        KeyCodeEventMapper.Instance.AddMappedKey(KeyCode.None, KeyCode.DownArrow, KeyState.HELD, () => { m_moveDir.z -= 1; }, "SimpleCamera2DMover -> Update the camera");

        KeyCodeEventMapper.Instance.AddMappedKey(KeyCode.None, KeyCode.RightArrow, KeyState.HELD, () => { m_moveDir.x += 1; }, "SimpleCamera2DMover -> Update the camera");
        KeyCodeEventMapper.Instance.AddMappedKey(KeyCode.None, KeyCode.LeftArrow, KeyState.HELD, () => { m_moveDir.x -= 1; }, "SimpleCamera2DMover -> Update the camera");

        FunctionUpdater.CreateUpdater(() => { Move(m_moveDir); }, "SimpleCamera2dMover - Move", true, UpdateCycle.LATE);
    }

    private void Move(Vector3 dir)
    {
        transform.position += m_moveDir * m_moveSpeed * Time.deltaTime;

        m_moveDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _screenCentre = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 _transformScreenPosition = m_viewCam.WorldToScreenPoint(transform.position);

        if (m_screenCentreBasedFacing) FaceMouse(_screenCentre);
        else FaceMouse(_transformScreenPosition);
    }

    private void FaceMouse(Vector3 anchorPosition)
    {
        Vector3 _mousePos = Input.mousePosition;

        Vector3 _mouseDir = _mousePos - anchorPosition;

        _mouseDir = Quaternion.Euler(90, 0, 0) * _mouseDir;
        _mouseDir.y = 0;

        transform.forward = _mouseDir;
    }

    private void FixedUpdate()
    {
        m_rigidBody.MovePosition(m_rigidBody.position + (m_moveDir * m_moveSpeed * Time.fixedDeltaTime));

        m_moveDir = Vector3.zero;
    }
}
