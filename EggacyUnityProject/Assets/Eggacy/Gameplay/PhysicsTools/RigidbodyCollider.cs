using UnityEngine;

namespace Eggacy.Gameplay.PhysicsTools
{
    public class RigidbodyCollider : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody = null;
        public new Rigidbody rigidbody => _rigidbody;
    }
}