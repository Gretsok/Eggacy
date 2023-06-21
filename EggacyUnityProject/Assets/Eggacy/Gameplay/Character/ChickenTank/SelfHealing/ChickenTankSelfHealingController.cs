using Eggacy.Gameplay.Combat.LifeManagement;
using Fusion;
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

        private void Update()
        {
            if(!Runner.IsServer) return;

            if(Time.time - _lastTimeOfHealing > _healingCooldown)
            {
                _lifeController.Heal(_healingAmount);
                _lastTimeOfHealing = Time.time;
            }
        }
    }
}