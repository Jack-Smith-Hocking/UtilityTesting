using UnityEngine;

namespace Jack.Utility.Physics
{
    /// <summary>
    /// Since Triggers and Collisions have no callback functionality (to my knowledge) i made a simple class to do just that
    /// Has a set of UnityEvents with no arguments and Actions that take the appropriate arguments 
    /// </summary>
    public class CollisionCallback : PhysicsCallback<Collision>
    {
        private void OnCollisionEnter(Collision collision)
        {
            OnEnter(collision);
        }
        private void OnCollisionStay(Collision collision)
        {
            OnStay(collision);
        }
        private void OnCollisionExit(Collision collision)
        {
            OnExit(collision);
        }
    }
}