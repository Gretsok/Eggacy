using Eggacy.Gameplay.Combat.Weapon;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG
{
    public class RPGWeapon : AWeapon
    {
        [SerializeField]
        private float _cooldown = 5f;
        private float _lastTimeOfShoot = float.MinValue;
        [SerializeField]
        private RPGRocket _rocketPrefab = null;

        public bool canShoot => Time.time - _lastTimeOfShoot >= _cooldown;

        protected override void HandlePrimaryAttackStarted(Vector3 aimSource, Vector3 aimDirection)
        {
            base.HandlePrimaryAttackStarted(aimSource, aimDirection);

            if (Time.time - _lastTimeOfShoot < _cooldown) return;

            _lastTimeOfShoot = Time.time;
            Shoot(aimSource, aimDirection);
        }

        private void Shoot(Vector3 aimSource, Vector3 aimDirection)
        {
            var rocket = Runner.Spawn(_rocketPrefab, aimSource, Quaternion.LookRotation(aimDirection, Vector3.up));
            rocket.SetReferencesForWeaponContainer(referencesForWeaponContainer as EggChampionReferencesForWeaponContainer);
        }
    }
}