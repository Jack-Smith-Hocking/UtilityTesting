using UnityEngine;

namespace GDML.Physics
{
    public class TriggerCallback : PhysicsCallback<Collider>
    {
        private void OnTriggerEnter(Collider other)
        {
            OnEnter(other);
        }
        private void OnTriggerStay(Collider other)
        {
            OnStay(other);
        }
        private void OnTriggerExit(Collider other)
        {
            OnExit(other);
        }
    }
}