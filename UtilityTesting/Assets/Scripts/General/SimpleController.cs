using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    public bool m_screenCentreBasedFacing = true;
    public float m_moveSpeed = 6;

    private Rigidbody m_rigidBody;
    private Camera m_viewCam;

    private Vector3 m_velocity;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_viewCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _screenCentre = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 _transformScreenPosition = m_viewCam.WorldToScreenPoint(transform.position);

        if (m_screenCentreBasedFacing) FaceMouse(_screenCentre);
        else FaceMouse(_transformScreenPosition);

        m_velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * m_moveSpeed;
    }

    private void FaceMouse(Vector3 anchorPosition)
    {
        Vector3 _mousePos = Input.mousePosition;

        Vector3 _mouseDir = _mousePos - anchorPosition;

        _mouseDir = Quaternion.Euler(90, 0, 0) * _mouseDir;
        _mouseDir.y = 0;

        //Debug.DrawRay(transform.position, transform.position + (_mouseDir * 10));

        transform.forward = _mouseDir;
    }

    private void FixedUpdate()
    {
        m_rigidBody.MovePosition(m_rigidBody.position + (m_velocity * Time.fixedDeltaTime));
    }
}
