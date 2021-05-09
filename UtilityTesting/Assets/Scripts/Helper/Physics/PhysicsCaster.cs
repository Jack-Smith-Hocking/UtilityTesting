using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDML.Physics
{
    public abstract class PhysicsCaster : MonoBehaviour
    {
        [Header("Base - Cast Settings")]

        [SerializeField]
        [Tooltip("The physics layers to detect objects on")] protected LayerMask m_castLayers;
        
        [SerializeField]
        [Tooltip("Transforms to cast from, the forward is used for direction")] protected List<Transform> m_castPoints = new List<Transform>();

        [Header("Base - Gizmo Settings")]

        [SerializeField]
        [Tooltip("Disable/enable the gizmo debugging")] protected bool m_showGizmo = true;

        [SerializeField]
        [Tooltip("Colour of the gizmos")] protected Color m_gizmoColour = Color.red;

        public List<RaycastHit> GetAllCastHits()
        {
            List<RaycastHit> _castHits = null;

            foreach (Transform castPoint in m_castPoints)
            {
                if (_castHits == null)
                {
                    _castHits = new List<RaycastHit>(GetCastHits(castPoint));
                    continue;
                }

                _castHits.AddRange(GetCastHits(castPoint));
            }

            return _castHits;
        }

        public RaycastHit[] GetCastHits(Vector3 pos, Vector3 dir) { return GetCastHits(new Ray(pos, dir)); }
        public RaycastHit[] GetCastHits(Transform castPoint) { return GetCastHits(new Ray(castPoint.position, castPoint.forward)); }

        public abstract RaycastHit[] GetCastHits(Ray castRay);
    }
}