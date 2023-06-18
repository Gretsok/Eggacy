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
                onShoot?.Invoke();
                _lastTimeOfShoot = Time.time;
            }
        }

        protected virtual void Shoot()
        {
            
        }
    }
}