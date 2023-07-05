using Fusion;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.Weapon
{
    public class AWeapon : NetworkBehaviour
    {
        private Vector3 _aimSource = default, _aimDirection = default;
        public Vector3 aimSource => _aimSource;
        public Vector3 aimDirection => _aimDirection;

        private IReferencesForWeaponContainer _referencesContainer = null;
        public IReferencesForWeaponContainer referencesForWeaponContainer => _referencesContainer;

        public Action onPrimaryAttackStarted = null;
        public Action onSecondaryAttackStarted = null;
        public Action onPrimaryAttackStopped = null;
        public Action onSecondaryAttackStopped = null;
        public Action onAttack_ServerOnly = null;
        public Action onAttack = null;

        public void SetReferencesContainer(IReferencesForWeaponContainer referencesContainer)
        {
            _referencesContainer = referencesContainer;
        }

        public void StartPrimaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            HandlePrimaryAttackStarted(aimSource, aimDirection);
            onPrimaryAttackStarted?.Invoke();
        }

        public void StopPrimaryAttack()
        {
            HandlePrimaryAttackStopped();
            onPrimaryAttackStopped?.Invoke();
        }

        protected virtual void HandlePrimaryAttackStarted(Vector3 aimSource, Vector3 aimDirection)
        { }

        protected virtual void HandlePrimaryAttackStopped()
        { }

        public void StartSecondaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            HandleSecondaryAttackStarted(aimSource, aimDirection);
            onSecondaryAttackStarted?.Invoke();
        }

        public void StopSecondaryAttack()
        {
            HandleSecondaryAttackStopped();
            onSecondaryAttackStopped?.Invoke();
        }

        protected virtual void HandleSecondaryAttackStarted(Vector3 aimSource, Vector3 aimDirection)
        { }
        protected virtual void HandleSecondaryAttackStopped()
        { }

        private void Update()
        {
            if (Runner.IsPlayer)
                PlayerUpdate();
            if (Runner.IsServer)
                ServerUpdate();
        }

        protected virtual void ServerUpdate()
        {
            
        }

        protected virtual void PlayerUpdate()
        {
            
        }

        internal void SetAim(Vector3 aimSource, Vector3 aimDirection)
        {
            _aimSource = aimSource;
            _aimDirection = aimDirection;
        }
    }
}