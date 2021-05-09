using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDML.Physics
{
    public class SphereCaster : PhysicsCaster
    {
        [Header("SphereCast - Cast Settings")]
        [Tooltip("The maximum distance to cast to")] [Min(0)] public float m_castDistance = 1;
        [Tooltip("The radius of the sphere cast")] [Min(0)] public float m_castRadius = 1;

        [Header("SphereCast - Gizmo Settings")]
        [Tooltip("Amount of straight lines connecting the start and end spheres")] [Min(0)] public int m_connectorCount = 4;

        #region Gizmo
        private void OnDrawGizmos()
        {
            if (!m_showGizmo) return;

            Color _prevColour = Gizmos.color;
            Gizmos.color = m_gizmoColour;

            Vector3 _origin = Vector3.zero;
            Vector3 _end = Vector3.zero;

            foreach (Transform vPoint in m_castPoints)
            {
                _origin = vPoint.position;
                _end = vPoint.position + (vPoint.forward * m_castDistance);

                Gizmos.DrawWireSphere(_origin, m_castRadius);
                Gizmos.DrawWireSphere(_end, m_castRadius);

                Gizmos.DrawLine(_origin, _end);

                DrawConnectors(_origin, _end, vPoint);
            }

            Gizmos.color = _prevColour;
        }

        private void DrawConnectors(Vector3 startPos, Vector3 endPos, Transform castPoint)
        {
            Vector3 _direction = Vector3.zero;

            for (int i = 0; i < m_connectorCount; i++)
            {
                _direction = castPoint.up;

                // Calculate where to start the line from on the surface of the sphere
                _direction = Quaternion.AngleAxis((360f / (float)m_connectorCount) * i, castPoint.forward) * _direction;

                Gizmos.DrawLine(startPos + (_direction * m_castRadius), endPos + (_direction * m_castRadius));
            }
        }

        #endregion

        public override RaycastHit[] GetCastHits(Ray castRay)
        {
            return UnityEngine.Physics.SphereCastAll(castRay, m_castRadius, m_castDistance, m_castLayers.value);
        }
    }
}