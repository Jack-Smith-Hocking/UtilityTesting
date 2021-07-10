using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jack.Utility;

using Sirenix.OdinInspector;

public class RotateAroundObject : MonoBehaviour
{
    #region General Settings
    [BoxGroup("General"), Tooltip("The Transform to rotate around")]
    [SceneObjectsOnly, Required]
    [SerializeField] private Transform m_rotationAnchor;

    [BoxGroup("General"), Tooltip("Whether to snap to the desired start rotation on Start")]
    [SerializeField] private bool m_snapToPositionOnStart = true;
    #endregion

    #region Rotation Settings
    [TabGroup("Rotation"), Tooltip("Spacial offset from the RotationAnchor")]
    [SerializeField] private Vector3 m_offset;
    
    [Space]

    [TabGroup("Rotation"), LabelText("Start Rotation")]
    [SerializeField, Range(0, 360)] private float m_startRot = 0;

    [TabGroup("Rotation"), LabelText("Rotation Radius")]
    [SerializeField] private float m_rotRadius;

    #region Rotation Properties
    [PropertySpace]

    [TabGroup("Rotation"), LabelText("Total Current Rotation")]
    [ShowInInspector] private float TotalCurrentRot => Mathf.Repeat(m_currentRot + m_startRot, 360);

    [TabGroup("Rotation"), LabelText("Can Rotate"), ReadOnly]
    [ShowInInspector]
    public bool CanRotate { get; private set; } = true;
    #endregion
    #endregion

    #region Speed Settings
    [TabGroup("Speed"), Tooltip("The speed that the rotater will rotate around the anchor")]
    [SerializeField] private float m_rotateSpeed;

    [TabGroup("Speed"), Tooltip("The speed the rotater will use when catching up to the anchor")]
    [SerializeField] private float m_catchUpSpeed;
    #endregion

    #region Debug Settings
    [TabGroup("Debug"), Tooltip("Whether to show the debug settings and data")]
    [SerializeField] private bool m_showDebug = true;

    [Space]

    [Tooltip("How many line segments in the rotation cycle around the anchor")]
    [TabGroup("Debug"), ShowIf(nameof(m_showDebug))]
    [SerializeField, Min(0)] private int m_debugResolution = 36;

    [Space]

    [Tooltip("Show a sphere that rotates around the anchor")]
    [TabGroup("Debug"), ShowIf(nameof(m_showDebug))]
    [SerializeField] private bool m_showDebugSphere = true;

    [Tooltip("The radius of the debug sphere")]
    [TabGroup("Debug"), ShowIf("@" + nameof(m_showDebug) + " && " + nameof(m_showDebugSphere))]
    [SerializeField, Min(0)] private float m_debugSphereRadius = 5;

    [Space]

    #region Debug Colours
    [Title("Debug Colours")]

    [Tooltip("The colour of the cycle line around the anchor")]
    [TabGroup("Debug"), ShowIf(nameof(m_showDebug))]
    [SerializeField] private Color m_debugLineColor = new Color(1, 1, 1, 1);

    [Tooltip("Colour of the debug sphere")]
    [TabGroup("Debug"), ShowIf("@" + nameof(m_showDebug) + " && " + nameof(m_showDebugSphere))]
    [SerializeField] private Color m_debugSphereColour = new Color(1, 0, 0, 1);
    #endregion
    #endregion

    private float m_currentRot = 0;
    private float m_currentSpeed = 0;

    private Vector3 m_targetPos;
    private Vector3 m_objectAxis = new Vector3(1, 0, 0);
    private Vector3 m_rotateAxis = new Vector3(0, 1, 0);

    private void OnDrawGizmos()
    {
        if (!m_showDebug || m_rotationAnchor == null) return;

        Color _gizmoCol = Gizmos.color;

        Gizmos.color = m_debugLineColor;

        float _currentRot = 0;

        Vector3 _firstPos = Vector3.zero;
        Vector3 _currentPos = Vector3.zero;
        Vector3 _prevPos = Vector3.zero;

        for (int _connector = 0; _connector < m_debugResolution; _connector++)
        {
            _currentRot += (360f / (float)m_debugResolution);

            Vector3 _pos = CalculateTargetPos(_currentRot);

            _prevPos = _currentPos;
            _currentPos = _pos;

            if (_connector == 0)
            {
                _firstPos = _currentPos;
                continue;
            }

            Gizmos.DrawLine(_prevPos, _currentPos);
        }

        Gizmos.DrawLine(_currentPos, _firstPos);

        DrawDebugPos();
        
        Gizmos.color = _gizmoCol;
    }

    private void DrawDebugPos()
    {
        if (!m_showDebugSphere) return;

        Gizmos.color = m_debugSphereColour;

        Vector3 _currentPos = Application.isEditor ? CalculateTargetPos(TotalCurrentRot) : m_targetPos;

        Gizmos.DrawSphere(_currentPos, m_debugSphereRadius);
    }

    private void Start()
    {
        if (!m_snapToPositionOnStart) return;
        if (m_rotationAnchor == null) return;

        transform.position = m_targetPos = CalculateTargetPos(m_currentRot + m_startRot);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePosition();
    }

    /// <summary>
    /// Calculate the position the rotater should be aiming for
    /// </summary>
    /// <param name="currentRot">Current rotation to aim for</param>
    /// <returns></returns>
    private Vector3 CalculateTargetPos(float currentRot)
    {
        Vector3 _dir = MathUtil.RotateBy(currentRot, m_rotateAxis, m_objectAxis);
        _dir = m_rotationAnchor.TransformDirection(_dir);

        return m_rotationAnchor.position + m_offset + (_dir * m_rotRadius);
    }

    private void UpdatePosition()
    {
        if (!CanRotate || m_rotationAnchor == null) return;

        bool _inDistance = DistUtil.InDistance(transform.position, m_targetPos, 0.1f);

        m_currentRot += _inDistance ? Time.fixedDeltaTime * m_rotateSpeed : 0;
        m_currentSpeed = _inDistance ? m_rotateSpeed : m_catchUpSpeed;

        m_targetPos = CalculateTargetPos(TotalCurrentRot);

        transform.position = Vector3.MoveTowards(transform.position, m_targetPos, m_currentSpeed * Time.fixedDeltaTime);
    }
}
