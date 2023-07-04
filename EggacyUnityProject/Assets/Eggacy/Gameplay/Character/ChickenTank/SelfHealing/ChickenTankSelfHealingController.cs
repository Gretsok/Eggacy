using Eggacy.Gameplay.Combat.LifeManagement;
using Fusion;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank.SelfHealing
{
    public class ChickenTankSelfHealingController : NetworkBehaviour
    {
        [SerializeField]
        private LifeController _lifeController = null;

        [SerializeField]
        private int _healingAmount = 2;
        [SerializeField]
        private float _healingCooldown = 1f;

        private float _lastTimeOfHealing = float.MinValue;

        [SerializeField]
        private int _damageReceivedToGoInBattle = 40;
        [SerializeField]
        private int _damageReceivedDescreasingSpeed = 20;
        private float _damageReceived = 0;

        private void Start()
        {
            _lifeController.onDamageDealt_ServerOnly += HandleDamageDealt;
        }

        private void HandleDamageDealt(LifeController arg1, int arg2, LifeController arg3)
        {
            _damageReceived += arg2;
        }

        private void OnDestroy()
        {
            if(_lifeController)
                _lifeController.onDamageDealt_ServerOnly += HandleDamageDealt;
        }

        private void Update()
        {
            if (!Runner) return;
            if(!Runner.IsServer) return;

            _damageReceived -= _damageReceivedDescreasingSpeed * Time.deltaTime;
            if(_damageReceived < 0)
            {
                _damageReceived = 0;

                if (Time.time - _lastTimeOfHealing > _healingCooldown)
                {
                    _lifeController.Heal(_healingAmount);
                    _lastTimeOfHealing = Time.time;
                }
            }   
        }
    }
}