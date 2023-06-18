using Eggacy.Gameplay.Combat.Weapon;
using UnityEngine;
using Fusion;
using System;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.Rifle
{
    public abstract class ARifleWeapon : AWeapon
    {
        [SerializeField]
        private float _baseCooldown = 0.5f;
        public float cooldown => _baseCooldown;
        [SerializeField]
        private int _baseDamageAmount = 15;
        public int damageAmount => _baseDamageAmount;

        [Networked]
        private bool _isShooting { get; set; }

        private float _lastTimeOfShoot = float.MinValue;

        public Action onShoot = null;
        public Action onShoot_serverOnly = null;


        protected override void HandlePrimaryAttackStarted(Vector3 aimSource, Vector3 aimDirection)
        {
            base.HandlePrimaryAttackStarted(aimSource, aimDirection);
            _isShooting = true;
            TryToShoot();
        }

        protected override void HandlePrimaryAttackStopped()
        {
            base.HandlePrimaryAttackStopped();
            _isShooting = false;
        }

        protected override void ServerUpdate()
        {
            base.ServerUpdate();
            if(_isShooting)
            {
                TryToShoot();
            }
        }

        private void TryToShoot()
        {
            if (Time.time - _lastTimeOfShoot > cooldown)
            {
                Shoot();
                NotifyOnShoot();
                _lastTimeOfShoot = Time.time;
            }
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

        protected virtual void Shoot()
        {
            
        }
    }
}