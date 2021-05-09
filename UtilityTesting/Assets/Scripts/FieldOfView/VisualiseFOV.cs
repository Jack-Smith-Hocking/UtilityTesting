using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Utility.FOV
{
    public struct EdgeInfo
    {
        public bool minIsSet;
        public bool maxIsSet;

        public Vector3 minPoint;
        public Vector3 maxPoint;

        public EdgeInfo(Vector3 min, Vector3 max, bool minSet = true, bool maxSet = true)
        {
            minPoint = min;
            maxPoint = max;

            this.minIsSet = minSet;
            this.maxIsSet = maxSet;
        }
    }
    public struct ViewCastInfo
    {
        public float angle;
        public float hitDist;

        public bool didHit;

        public Vector3 hitPoint;
        public Vector3 direction;

        public ViewCastInfo(float angle, float hitDist, bool didHit, Vector3 hitPoint, Vector3 dir)
        {
            this.angle = angle;
            this.hitDist = hitDist;
            this.didHit = didHit;

            this.hitPoint = hitPoint;
            direction = dir;
        }
    }

    public class VisualiseFOV : MonoBehaviour
    {
        [SerializeField, Required, InlineEditor] private CalculateFOV m_fovCalculator;

        #region Mesh Settings
        [TabGroup("Mesh"), Tooltip("Used for creation of mesh")]
        [SerializeField] private MeshFilter m_viewMeshFilter;

        [Space]

        [TabGroup("Mesh"), Tooltip("If set to true, even targets will interrupt vision")]
        [SerializeField] private bool m_cutTargetsFromView = false;

        [TabGroup("Mesh"), Tooltip("How much of an obstacle to show")]
        [SerializeField, Min(0)] private float m_maskCutAwayDistance = 0.1f;
        [TabGroup("Mesh"), Tooltip("How accurate the generation of the mesh will be")]
        [SerializeField, Min(0)] private float m_meshResolution = 4;
        #endregion

        #region Edge Settings
        [TabGroup("Edge"), Tooltip("How many iterations for edge detection, higher values will decrease stuttering when looking at edges")]
        [SerializeField, Min(0)] private int m_edgeResolveIterations = 6;
        [TabGroup("Edge"), Tooltip("Distance threshold between two or more edges")]
        [SerializeField, Min(0)] private float m_edgeDistanceThreshold = 0.25f;
        #endregion

        #region CalculateFOV Variables
        private float ViewRadius => m_fovCalculator.ViewRadius;
        private float ViewAngle => m_fovCalculator.ViewAngle;

        private int TargetMask => m_fovCalculator.TargetMask;
        private int ObstacleMask => m_fovCalculator.ObstacleMask;
        #endregion

        private List<Vector3> m_viewPoints = new List<Vector3>();

        private Mesh m_viewMesh;

        private void Start()
        {
            m_viewMesh = new Mesh();
            m_viewMesh.name = "ViewMesh";
            m_viewMeshFilter.mesh = m_viewMesh;
        }

        /// <summary>
        /// Updates the calculation of view points, then updates the mesh
        /// </summary>
        private void UpdateViewPointCalculation()
        {
            int _stepCount = Mathf.RoundToInt(ViewAngle * m_meshResolution);
            float _stepAngleSize = ViewAngle / _stepCount;

            ViewCastInfo _oldViewCastInfo = new ViewCastInfo();

            m_viewPoints.Clear();
            m_viewPoints.Capacity = _stepCount;

            for (int _stepIndex = 0; _stepIndex <= _stepCount; _stepIndex++)
            {
                float _angle = transform.eulerAngles.y - (ViewAngle / 2) + (_stepAngleSize * _stepIndex);

                ViewCastInfo _newViewCastInfo = CalculateViewCast(_angle);

                if (_stepIndex > 0)
                {
                    EdgeCalculation(_oldViewCastInfo, _newViewCastInfo);
                }

                m_viewPoints.Add(_newViewCastInfo.hitPoint);

                _oldViewCastInfo = _newViewCastInfo;
            }

            GenerateMesh();
        }

        /// <summary>
        /// Calculate an accurate edge based on two view points
        /// </summary>
        /// <param name="oldViewCastInfo">First view point</param>
        /// <param name="newViewCastInfo">Second view point</param>
        private void EdgeCalculation(ViewCastInfo oldViewCastInfo, ViewCastInfo newViewCastInfo)
        {
            bool _edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCastInfo.hitDist - newViewCastInfo.hitDist) > m_edgeDistanceThreshold;
            bool _mutlipleHitsDetected = (oldViewCastInfo.didHit && newViewCastInfo.didHit && _edgeDistanceThresholdExceeded);

            if (oldViewCastInfo.didHit != newViewCastInfo.didHit || _mutlipleHitsDetected)
            {
                EdgeInfo _edge = CalculateEdgeValue(oldViewCastInfo, newViewCastInfo);

                if (_edge.minIsSet) m_viewPoints.Add(_edge.minPoint);
                if (_edge.maxIsSet) m_viewPoints.Add(_edge.maxPoint);
            }
        }

        /// <summary>
        /// Calculate a more accurate edge value based on two view points
        /// </summary>
        /// <param name="minView"></param>
        /// <param name="maxView"></param>
        /// <returns>An edge more accurately between two view points</returns>
        private EdgeInfo CalculateEdgeValue(ViewCastInfo minView, ViewCastInfo maxView)
        {
            float _minAngle = minView.angle;
            float _maxAngle = maxView.angle;

            Vector3 _minPoint = Vector3.zero;
            Vector3 _maxPoint = Vector3.zero;

            bool _minSet = false;
            bool _maxSet = false;

            // Calculate a view point between two others for better edge detection
            for (int _edgeIndex = 0; _edgeIndex < m_edgeResolveIterations; _edgeIndex++)
            {
                float _newAngle = (_minAngle + _maxAngle) / 2; // Calculate an angle half way between the min and max

                ViewCastInfo _newViewCastInfo = CalculateViewCast(_newAngle); // Calculate a new view point half way between the others
                bool _edgeDistanceThresholdExceeded = Mathf.Abs(minView.hitDist - _newViewCastInfo.hitDist) > m_edgeDistanceThreshold; // Calculate whether two edges are too far away

                if (_newViewCastInfo.didHit == minView.didHit && !_edgeDistanceThresholdExceeded)
                {
                    _minAngle = _newAngle;
                    _minPoint = _newViewCastInfo.hitPoint;

                    _minSet = true;

                    continue;
                }

                _maxAngle = _newAngle;
                _maxPoint = _newViewCastInfo.hitPoint;

                _maxSet = true;
            }

            return new EdgeInfo(_minPoint, _maxPoint, _minSet, _maxSet);
        }
        /// <summary>
        /// Cast a ray and calculate what it hits
        /// </summary>
        /// <param name="globalAngle">Angle to fire ray from</param>
        /// <returns></returns>
        private ViewCastInfo CalculateViewCast(float globalAngle)
        {
            RaycastHit _hitInfo;

            Vector3 _dir = m_fovCalculator.DirFromAngle(globalAngle, true);

            int _viewMask = m_cutTargetsFromView ? ObstacleMask | TargetMask : ObstacleMask; // Check what layers to treat as obstacles for the mesh

            bool _didHit = Physics.Raycast(transform.position, _dir, out _hitInfo, ViewRadius, _viewMask);

            Vector3 _point = _didHit ? _hitInfo.point : transform.position + (_dir * ViewRadius);
            float _dist = _didHit ? _hitInfo.distance : ViewRadius;

            return new ViewCastInfo(globalAngle, _dist, _didHit, _point, _dir);
        }

        /// <summary>
        /// Create a mesh and set the vertices to visualise the FOV
        /// </summary>
        private void GenerateMesh()
        {
            if (m_viewPoints.Count == 0) return;

            int _vertexCount = m_viewPoints.Count + 1;
            int[] _triangles = new int[(_vertexCount - 2) * 3];

            Vector3[] _vertices = new Vector3[_vertexCount];

            _vertices[0] = Vector3.zero;

            for (int _vertexIndex = 0; _vertexIndex < _vertexCount - 1; _vertexIndex++)
            {
                Vector3 _point = m_viewPoints[_vertexIndex];

                Vector3 _cutOffDir = (_point - transform.position).normalized;
                _vertices[_vertexIndex + 1] = transform.InverseTransformPoint(_point + _cutOffDir * m_maskCutAwayDistance);

                if (_vertexIndex >= (_vertexCount - 2)) continue;

                _triangles[_vertexIndex * 3] = 0;
                _triangles[_vertexIndex * 3 + 1] = _vertexIndex + 1;
                _triangles[_vertexIndex * 3 + 2] = _vertexIndex + 2;
            }

            m_viewMesh.Clear();
            m_viewMesh.vertices = _vertices;
            m_viewMesh.triangles = _triangles;
            m_viewMesh.RecalculateNormals();
        }

        private void LateUpdate()
        {
            UpdateViewPointCalculation();
        }
    }
}