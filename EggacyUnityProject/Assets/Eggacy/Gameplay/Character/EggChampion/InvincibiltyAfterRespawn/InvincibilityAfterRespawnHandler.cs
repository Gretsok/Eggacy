using Eggacy.Gameplay.Combat.LifeManagement;
using Fusion;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.InvincibilityAfterRespawn
{
    public class InvincibilityAfterRespawnHandler : NetworkBehaviour
    {
        [SerializeField]
        private LifeController _lifeController = null;
        [SerializeField]
        private EggChampionCharacter _character = null;
        [SerializeField]
        private float _timeOfInvincibilityAfterRespawn = 3f;

        public Action onInvincibilityStarted = null;
        public Action onInvincibilityStopped = null;

        private void Start()
        {
            _character.onRespawned += HandleRespawned;
        }

        private void OnDestroy()
        {
            if(_character)
                _character.onRespawned -= HandleRespawned;
        }

        private void HandleRespawned(EggChampionCharacter character)
        {
            _lifeController.SetCanTakeDamage(false);
            Invoke(nameof(DeactivateInvincibility), _timeOfInvincibilityAfterRespawn);
            Rpc_NotifyInvincibilityStarted();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyInvincibilityStarted()
        {
            onInvincibilityStarted?.Invoke();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyInvincibilityStopped()
        {
            onInvincibilityStopped?.Invoke();
        }

        private void DeactivateInvincibility()
        {
            _lifeController.SetCanTakeDamage(true);
            Rpc_NotifyInvincibilityStopped();
        }
    }
}