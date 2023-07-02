using Eggacy.Gameplay.Combat.Weapon;
using Fusion;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG
{
    public class RPGWeapon : AWeapon
    {
        [SerializeField]
        private float _cooldown = 5f;
        [Networked]
        private float _lastTimeOfShoot { get; set; } = float.MinValue;
        [SerializeField]
        private RPGRocket _rocketPrefab = null;

        public bool canShoot => Runner.InterpolationRenderTime - _lastTimeOfShoot >= _cooldown;

        public Action onShoot = null;
        public Action onShoot_serverOnly = null;


        public override void Spawned()
        {
            base.Spawned();
            _lastTimeOfShoot = float.MinValue;
        }

        protected override void HandlePrimaryAttackStarted(Vector3 aimSource, Vector3 aimDirection)
        {
            base.HandlePrimaryAttackStarted(aimSource, aimDirection);

            if (Runner.InterpolationRenderTime - _lastTimeOfShoot < _cooldown) return;

            _lastTimeOfShoot = Runner.InterpolationRenderTime;
            Shoot(aimSource, aimDirection);
        }

        private void Shoot(Vector3 aimSource, Vector3 aimDirection)
        {
            var rocket = Runner.Spawn(_rocketPrefab, aimSource, Quaternion.LookRotation(aimDirection, Vector3.up));
            rocket.SetReferencesForWeaponContainer(referencesForWeaponContainer as EggChampionReferencesForWeaponContainer);
            NotifyOnShoot();
        }

        private void NotifyOnShoot()
        {
            onShoot_serverOnly?.Invoke();
            Rpc_NotifyOnShoot();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyOnShoot()
        {
            onShoot?.Invoke();
        }


    }
}