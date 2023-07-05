using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Level
{
    public class HealSpawner : NetworkBehaviour
    {
        [SerializeField]
        private GameObject _weaponModelControllerPrefab = null;
        public GameObject weaponModelControllerPrefab => _weaponModelControllerPrefab;

        [SerializeField]
        private float _cooldownToRespawn = 20f;
        public float cooldownToRespawn => _cooldownToRespawn;

        [SerializeField]
        private float _healAmount = 50f;
        public float healAmount => _healAmount;

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

            if (!_isActive)
            {
                if (Runner.InterpolationRenderTime - _lastTimeOfPickUp > _cooldownToRespawn)
                {
                    RespawnHeal();
                }
            }
        }

        private void RespawnHeal()
        {
            _isActive = true;
        }

        public GameObject TryToGetHealPickedUp()
        {
            if (!isActive) return null;
            _isActive = false;
            _lastTimeOfPickUp = Runner.InterpolationRenderTime;

            return _weaponModelControllerPrefab;
        }
    }
}

