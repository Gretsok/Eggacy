using Eggacy.Gameplay.Combat.LifeManagement;
using Eggacy.Gameplay.Combat.TeamManagement;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank.Shooting
{
    public class ChickenTankShootingController : NetworkBehaviour
    {
        [SerializeField]
        private TeamController _teamController = null;

        [Networked]
        private Quaternion _turretRotationTarget {get;set;}
        public Quaternion turretRotationTarget => _turretRotationTarget;
        [Networked]
        private Quaternion _currentTurretRotation { get;set;}
        [SerializeField]
        private Transform _turretModelRoot = null;
        [SerializeField]
        private float _rotationSmoothness = 8f;
        [SerializeField]
        private float _scanRadius = 10f;

        [Networked]
        private bool _isShooting { get;set;}
        [Networked]
        private bool _isInitialized { get; set; }

        public override void Spawned()
        {
            base.Spawned();

            if (!Runner.IsServer) return;
            _currentTurretRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            _turretRotationTarget = _currentTurretRotation;
            _turretModelRoot.rotation = _currentTurretRotation;
            _isInitialized = true;
        }

        public override void FixedUpdateNetwork() 
        {
            base.FixedUpdateNetwork();

            _currentTurretRotation = Quaternion.Lerp(_currentTurretRotation, _turretRotationTarget, Runner.DeltaTime * _rotationSmoothness);
            _turretModelRoot.rotation = _currentTurretRotation;
        }

        private void Update()
        {
            if (!Runner) return;
            if (!Runner.IsServer) return;
            if(!_isInitialized) return;

            var collidersInContact = Physics.OverlapSphere(_turretModelRoot.position, _scanRadius);
            LifeControllerCollider closestValidTarget = null;
            float sqrDistanceToClosestValidTarget = float.MaxValue;
            for(int i = 0; i < collidersInContact.Length; ++i)
            {
                if (collidersInContact[i].TryGetComponent(out LifeControllerCollider lifeControllerCollider))
                {
                    if(lifeControllerCollider.lifeController.teamController.teamData != _teamController.teamData)
                    {
                        var sqrDistanceToCollider = (transform.position - lifeControllerCollider.transform.position).sqrMagnitude;
                        if(sqrDistanceToCollider < sqrDistanceToClosestValidTarget)
                        {
                            sqrDistanceToClosestValidTarget = sqrDistanceToCollider;
                            closestValidTarget = lifeControllerCollider;
                        }
                    }
                }
            }

            if(closestValidTarget)
                _turretRotationTarget = Quaternion.LookRotation((closestValidTarget.transform.position - _turretModelRoot.position).normalized, Vector3.up);
            else
                _turretRotationTarget = Quaternion.LookRotation(transform.forward, Vector3.up);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Runner) return;
            if (!Runner.IsServer) return;
            if (!_isInitialized) return;

            Gizmos.color = _isShooting ? Color.red : Color.green;
            Gizmos.DrawWireSphere(_turretModelRoot.position, _scanRadius);
        }
#endif
    }
}