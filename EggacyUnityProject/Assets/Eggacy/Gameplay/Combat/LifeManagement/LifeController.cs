using Fusion;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.LifeManagement
{
    public class LifeController : NetworkBehaviour
    {
        [SerializeField]
        private TeamManagement.TeamController _teamController = null;
        [SerializeField]
        private int _baseMaxLife = 100;
        public int maxLife => _baseMaxLife;
        [Networked]
        private int _currentLife { get; set; }
        public int currentLife => _currentLife;

        public Action<LifeController> onDied = null;
        public Action<LifeController, int> onDamageTaken = null;
        public Action<LifeController, int> onHealed = null;
        public Action<LifeController> onDied_ServerOnly = null;
        public Action<LifeController, int> onDamageTaken_ServerOnly = null;
        public Action<LifeController, int> onHealed_ServerOnly = null;

        public override void Spawned()
        {
            base.Spawned();
            if(Runner.IsServer)
            {
                _currentLife = maxLife;
            }
        }

        public void TakeDamage(Damage.Damage damage)
        {
            if(!Runner.IsServer) return;

            // If it doesn't do damage, we completely ignore it
            if (damage.amountToRetreat == 0) return;
            // No friendly fire
            if (_teamController.teamData.team == damage.teamSource) return;
            // Even if we have friendly fire, we cannot shoot ourselves
            if (damage.source == gameObject) return;

            // We ensure that we manage only positive values
            if(damage.amountToRetreat < 0)
            {
                damage.amountToRetreat = Mathf.Abs(damage.amountToRetreat);
            }

            var rawLifeAfterDamage = _currentLife - damage.amountToRetreat;

            if(rawLifeAfterDamage < 0) // If the life controller dies after the damage
            {
                var realAmountOfDamage = rawLifeAfterDamage - _currentLife;
                _currentLife = 0;
                NotifyDamageTaken(realAmountOfDamage);
                NotifyDied();
            }
            else // If the life controller survives after the damage
            {
                _currentLife -= damage.amountToRetreat;
                NotifyDamageTaken(damage.amountToRetreat);
            }
        }

        private void NotifyDamageTaken(int damageTaken)
        {
            onDamageTaken_ServerOnly?.Invoke(this, damageTaken);
            Rpc_NotifyDamageTaken(damageTaken);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyDamageTaken(int damageTaken)
        {
            onDamageTaken?.Invoke(this, damageTaken);
        }

        private void NotifyDied()
        {
            onDied_ServerOnly?.Invoke(this);
            Rpc_NotifyDied();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyDied()
        {
            onDied?.Invoke(this);
        }

        private void NotifyHealed(int amountHealed)
        {
            onHealed_ServerOnly?.Invoke(this, amountHealed);
            Rpc_NotifyHealed(amountHealed);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyHealed(int amountHealed)
        {
            onHealed?.Invoke(this, amountHealed);
        }
    }
}