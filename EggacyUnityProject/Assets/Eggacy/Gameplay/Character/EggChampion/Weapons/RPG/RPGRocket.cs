using Eggacy.Gameplay.Combat.Damage;
using Eggacy.Gameplay.Combat.LifeManagement;
using Fusion;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG
{
    public class RPGRocket : NetworkBehaviour
    {
        [SerializeField]
        private NetworkRigidbody _rigidbody = null;

        [SerializeField]
        private float _travelSpeed = 12f;

        [SerializeField]
        private float _maxLifeDuration = 8f;
        private float _timeOfspawn = float.MaxValue;

        [SerializeField]
        private LayerMask _collisionToTriggerExplosion = default;

        [Header("Explosion")]
        [SerializeField]
        private Transform _explosionSource = null;
        public Transform explosionSource => _explosionSource;
        [SerializeField]
        private float _explosionRadius = 5f;
        [SerializeField]
        private float _maxPropulsionForce = 8f;
        [SerializeField]
        private float _maxHeightPropulsionForceMultiplier = 2f;
        [SerializeField]
        private int _maxDamageToDeal = 80;

        private EggChampionReferencesForWeaponContainer _referencesForWeaponContainer = null;

        public Action onExploded = null;

        public void SetReferencesForWeaponContainer(EggChampionReferencesForWeaponContainer referencesForWeaponContainer)
        {
            _referencesForWeaponContainer = referencesForWeaponContainer;
        }

        public override void Spawned()
        {
            base.Spawned();
            _rigidbody.Rigidbody.AddForce(transform.forward *  _travelSpeed, ForceMode.VelocityChange);
            _timeOfspawn = Time.time;
        }

        private void Update()
        {
            if (!Runner.IsServer) return;

            if(Time.time - _timeOfspawn > _maxLifeDuration)
            {
                HandleEndOfLifeReached();
            }

        }

        private void HandleEndOfLifeReached()
        {
            Explode();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Runner) return;
            if (!Runner.IsServer) return;
            if (other.TryGetComponent(out LifeControllerCollider lifeControllerCollider)                
                && lifeControllerCollider.lifeController.gameObject == _referencesForWeaponContainer.teamController.gameObject) return;

            if (_collisionToTriggerExplosion == (_collisionToTriggerExplosion | (1 << other.gameObject.layer)))
            {
                Explode();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!Runner) return;
            if (!Runner.IsServer) return;
            if (collision.gameObject.TryGetComponent(out LifeControllerCollider lifeControllerCollider)
                && lifeControllerCollider.lifeController.gameObject == _referencesForWeaponContainer.teamController.gameObject) return;

            if (_collisionToTriggerExplosion == (_collisionToTriggerExplosion | (1 << collision.gameObject.layer)))
            {
                Explode();
            }
        }

        private void Explode()
        {
            if (!Runner.IsServer) return;

            var colliders = Physics.OverlapSphere(_explosionSource.position, _explosionRadius);

            for(int i = 0; i < colliders.Length; i++) 
            {
                float distanceRatio = 1 - Mathf.Clamp01(Vector3.Distance(_explosionSource.position, colliders[i].transform.position) / _explosionRadius);
                if (colliders[i].TryGetComponent(out LifeControllerCollider lifeControllerCollider))
                {
                    if(lifeControllerCollider.lifeController.TryGetComponent(out EggChampionCharacter character))
                    {
                        character.networkRigidbody.Rigidbody.AddExplosionForce(_maxPropulsionForce, _explosionSource.position, _explosionRadius, _maxHeightPropulsionForceMultiplier, ForceMode.Impulse);
                    }
                    Damage damageToDeal = new Damage();
                    damageToDeal.amountToRetreat = (int)(_maxDamageToDeal * distanceRatio * _referencesForWeaponContainer.globalMutatorsHandler.damageMultiplier);
                    damageToDeal.teamSource = _referencesForWeaponContainer.teamController.teamData.team;
                    damageToDeal.source = _referencesForWeaponContainer.teamController.gameObject;
                    lifeControllerCollider.lifeController.TakeDamage(damageToDeal);
                }
            }

            Runner.Despawn(Object);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            onExploded?.Invoke();
        }
    }
}