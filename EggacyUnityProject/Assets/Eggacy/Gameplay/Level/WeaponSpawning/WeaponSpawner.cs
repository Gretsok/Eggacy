using Eggacy.Gameplay.Combat.Weapon;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Level.WeaponSpawner
{
    public class WeaponSpawner : NetworkBehaviour
    {
        [SerializeField]
        private AWeaponModelController _weaponModelControllerPrefab = null;
        public AWeaponModelController weaponModelControllerPrefab => _weaponModelControllerPrefab;

        [SerializeField]
        private float _cooldownToRespawn = 20f;
        public float cooldownToRespawn => _cooldownToRespawn;

        [Networked]
        private bool _isActive { get; set; }
        public bool isActive => _isActive;

        [Networked]
        private float _lastTimeOfPickUp { get; set; }
        public float timePassedSincePickUp => Runner ? Runner.InterpolationRenderTime - _lastTimeOfPickUp : 0f;

        public override void Spawned()
        {
            base.Spawned();
            _lastTimeOfPickUp = float.MinValue;
            _isActive = false;
        }

        private void Update()
        {
            if (!Runner) return;
            if (!Runner.IsServer) return;
            
            if(!_isActive)
            {
                if(Runner.InterpolationRenderTime - _lastTimeOfPickUp > _cooldownToRespawn)
                {
                    RespawnWeapon();
                }
            }
        }

        private void RespawnWeapon()
        {
            _isActive = true;
        }

        public AWeaponModelController TryToGetWeaponPickedUp()
        {
            if(!isActive) return null;
            _isActive = false;
            _lastTimeOfPickUp = Runner.InterpolationRenderTime;

            return _weaponModelControllerPrefab;
        }
    }
}