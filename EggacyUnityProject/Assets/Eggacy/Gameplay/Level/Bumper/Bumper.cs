using Eggacy.Gameplay.PhysicsTools;
using UnityEngine;

namespace Eggacy.Gameplay.Level.Bumper
{
    public class Bumper : MonoBehaviour
    {
        [SerializeField, Tooltip("Dump will go along Y axis of this transform")]
        private Transform _bumpDirectionTransform = null;
        [SerializeField]
        private float _bumpAcceleration = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out RigidbodyCollider rigidbodyCollider))
            {
                rigidbodyCollider.rigidbody.AddForce(_bumpDirectionTransform.up * _bumpAcceleration, ForceMode.VelocityChange);
            }
        }
    }
}