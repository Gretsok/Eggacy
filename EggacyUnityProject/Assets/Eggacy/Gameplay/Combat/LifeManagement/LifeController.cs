using Fusion;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.LifeManagement
{
    public class LifeController : NetworkBehaviour
    {
        [SerializeField]
        private TeamManagement.TeamController _teamController = null;
        public TeamManagement.TeamController teamController => _teamController;

        [SerializeField]
        private int _baseMaxLife = 100;
        public int maxLife => _baseMaxLife + _bonusMaxLife;

        [Networked]
        private int _bonusMaxLife { get; set; }

        [Networked]
        private int _currentLife { get; set; }
        public int currentLife => _currentLife;

        public Action<LifeController> onDied = null;
        public Action<LifeController, int> onDamageTaken = null;
        public Action<LifeController, Vector3> onDamageTakenFromPosition = null;
        public Action<LifeController, int> onHealed = null;
        public Action<LifeController> onDied_ServerOnly = null;
        public Action<LifeController, int> onDamageTaken_ServerOnly = null;
        public Action<LifeController, int> onHealed_ServerOnly = null;
        public Action<LifeController, int> onDamageDealt = null;
        public Action<LifeController, int, LifeController> onDamageDealt_ServerOnly = null;
        public Action<LifeController> onKilled = null;
        public Action<LifeController, LifeController> onKilled_ServerOnly = null;

        public override void Spawned()
        {
            base.Spawned();
            ResetLife();
        }

        public void ResetBonus()
        {
            if (!Runner.IsServer) return;

            _bonusMaxLife = 0;
        }

        public void ResetLife()
        {
            if (!Runner.IsServer) return;

            var delta = maxLife - _currentLife;
            _currentLife = maxLife;
            NotifyHealed(delta);
        }

        public void TakeDamage(Damage.Damage damage)
        {
            if(!Runner.IsServer) return;

            // If it doesn't do damage, we completely ignore it
            if (damage.amountToRetreat == 0) return;
            // No friendly fire
            if (damage.teamSource != null && _teamController.teamData.team == damage.teamSource) return;
            // Even if we have friendly fire, we cannot shoot ourselves
            if (damage.source == gameObject) return;

            // We ensure that we manage only positive values
            if(damage.amountToRetreat < 0)
            {
                damage.amountToRetreat = Mathf.Abs(damage.amountToRetreat);
            }

            var rawLifeAfterDamage = _currentLife - damage.amountToRetreat;

            if(rawLifeAfterDamage <= 0) // If the life controller dies after the damage
            {
                var realAmountOfDamage = rawLifeAfterDamage - _currentLife;
                _currentLife = 0;
                NotifyDamageTaken(realAmountOfDamage, damage.source);
                NotifyDied();
            }
            else // If the life controller survives after the damage
            {
                _currentLife -= damage.amountToRetreat;
                NotifyDamageTaken(damage.amountToRetreat, damage.source);
            }

            if(damage.source is GameObject)
            {
                if((damage.source as GameObject).TryGetComponent(out LifeController sourceLifeController))
                {
                    sourceLifeController.NotifyDamageDealt(damage.amountToRetreat, this);
                    if (rawLifeAfterDamage <= 0)
                        sourceLifeController.NotifyKilled(this);
                }
            }
        }

        public void Heal(int amountToHeal)
        {
            if (!Runner.IsServer) return;

            if (_currentLife >= maxLife) return;

            _currentLife += amountToHeal;
            if(_currentLife > maxLife)
            {
                _currentLife = maxLife;
            }
            NotifyHealed(amountToHeal);
        }

        private void NotifyDamageDealt(int amountToRetreat, LifeController victim)
        {
            onDamageDealt_ServerOnly?.Invoke(this, amountToRetreat, victim);
            Rpc_NotifyDamageDealt(amountToRetreat);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyDamageDealt(int amountToRetreat)
        {
            onDamageDealt?.Invoke(this, amountToRetreat);
        }

        private void NotifyDamageTaken(int damageTaken, UnityEngine.Object source)
        {
            onDamageTaken_ServerOnly?.Invoke(this, damageTaken);
            Rpc_NotifyDamageTaken(damageTaken);
            if(source 
                && source is GameObject)
            {
                Rpc_NotifyDamageTakenFromPosition((source as GameObject).transform.position);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyDamageTaken(int damageTaken)
        {
            onDamageTaken?.Invoke(this, damageTaken);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyDamageTakenFromPosition(Vector3 position)
        {
            onDamageTakenFromPosition?.Invoke(this, position);
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

        private void NotifyKilled(LifeController victim)
        {
            onKilled_ServerOnly?.Invoke(this, victim);
            Rpc_NotifyKilled();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyKilled()
        {
            onKilled?.Invoke(this);
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

        public void SetBonusMaxLife(int bonusMaxLife)
        {
            if (!Runner.IsServer) return;

            if(bonusMaxLife < 0)
            {
                Debug.LogError("Bonus max life cannot be negative");
                _bonusMaxLife = 0;
                return;
            }

            _bonusMaxLife = bonusMaxLife;
        }
    }
}