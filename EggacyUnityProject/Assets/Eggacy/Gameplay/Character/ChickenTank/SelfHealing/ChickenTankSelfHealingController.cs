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
        private float _timeToWaitToHealAfterLeavingBattle = 1f;
        private float _timeOfLastHit = float.MinValue;
        [SerializeField]
        private int _damageReceivedToGoInBattle = 40;
        [SerializeField]
        private int _damageReceivedDescreasingSpeed = 20;
        private float _damageReceived = 0;

        private bool _isInBattle = false;

        private void Start()
        {
            _lifeController.onDamageTaken_ServerOnly += HandleDamageReceived;
        }

        private void HandleDamageReceived(LifeController arg1, int arg2)
        {
            _damageReceived += arg2;
            _timeOfLastHit = Runner.InterpolationRenderTime;
            if(_damageReceived >= _damageReceivedToGoInBattle)
            {
                _isInBattle = true;
            }
        }

        private void OnDestroy()
        {
            if(_lifeController)
                _lifeController.onDamageTaken_ServerOnly -= HandleDamageReceived;
        }

        private void Update()
        {
            if (!Runner) return;
            if(!Runner.IsServer) return;


            if (_isInBattle && Runner.InterpolationRenderTime - _timeOfLastHit > _timeToWaitToHealAfterLeavingBattle)
            {
                _damageReceived = 0;
                _isInBattle = false;
            }


            _damageReceived -= _damageReceivedDescreasingSpeed * Time.deltaTime;
            if (_damageReceived < 0)
            {
                _damageReceived = 0;
            }

            if (!_isInBattle)
            {
                if (Runner.InterpolationRenderTime - _lastTimeOfHealing > _healingCooldown)
                {
                    _lifeController.Heal(_healingAmount);
                    _lastTimeOfHealing = Runner.InterpolationRenderTime;
                }
            }
        }
    }
}