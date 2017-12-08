using UnityEngine;

namespace Neodroid.Utilities
{
    [RequireComponent(typeof(Collider))]
    public class ChildCollisionPublisher : MonoBehaviour
    {

        public delegate void OnCollisionDelegate(Collision collision);

        OnCollisionDelegate _collision_delegate;

        public OnCollisionDelegate CollisionDelegate {
            set { _collision_delegate = value; }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _collision_delegate(collision);
        }
    }
}