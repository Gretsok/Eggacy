using Eggacy.Gameplay.Combat.Weapon;
using Cinemachine.Utility;
using UnityEngine;
using Fusion;
using Eggacy.Gameplay.Combat.LifeManagement;
using System;
using System.Collections;

namespace Eggacy.Gameplay.Character.EggChampion
{
    public class EggChampionCharacter : NetworkBehaviour
    {
        [SerializeField]
        private NetworkRigidbody _rigidbody = null;
        public NetworkRigidbody networkRigidbody => _rigidbody;
        [SerializeField]
        private HoldedWeaponController _weaponController = null;
        public HoldedWeaponController weaponController => _weaponController;
        [SerializeField]
        private LifeController _lifeController = null;

        [SerializeField]
        private Transform _model = null;
        [SerializeField]
        private GameObject _collidersGO = null;

        [SerializeField]
        private float _jumpVelocity = 8f;
        [SerializeField]
        private float _movementSpeed = 5f;
        [SerializeField]
        private float _movementSmoothness = 8f;
        private Vector3 _directionToMove { get; set; }
        [Networked]
        private Vector3 _orientation { get; set; }

        [Networked]
        private Vector3 _currentPlannedVelocity { get; set; }

        [Networked(OnChanged = nameof(HandleIsAliveChanged))]
        private bool _isAlive { get; set; }

        private void Start()
        {
            _lifeController.onDied_ServerOnly += HandleDied_ServerOnly;

        }
        private void OnDestroy()
        {
            _lifeController.onDied_ServerOnly -= HandleDied_ServerOnly;
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (!_orientation.AlmostZero())
                _rigidbody.Rigidbody.rotation = Quaternion.LookRotation(_orientation, Vector3.up);

            float gravity = _rigidbody.Rigidbody.velocity.y;
            _currentPlannedVelocity = Vector3.Lerp(_currentPlannedVelocity, _directionToMove * _movementSpeed, Runner.DeltaTime * _movementSmoothness);
            _rigidbody.Rigidbody.velocity = _currentPlannedVelocity + gravity * Vector3.up;
        }

        #region Movement Behaviour
        public void SetDirectionToMove(Vector3 directionToMove)
        {
            _directionToMove = directionToMove;
        }

        public void SetOrientation(Vector3 orientation)
        {
            _orientation = orientation;
        }
        #endregion

        #region Attack Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StartPrimaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            if(_weaponController)
                _weaponController.StartPrimaryAttack(aimSource, aimDirection);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StartSecondaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            if (_weaponController)
                _weaponController.StartSecondaryAttack(aimSource, aimDirection);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StopPrimaryAttack()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            if (_weaponController)
                _weaponController.StopPrimaryAttack();
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StopSecondaryAttack()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            if (_weaponController)
                _weaponController.StopSecondaryAttack();
        }
        #endregion

        #region Rally Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_RequestRally()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            Rpc_HandleRallyFeedback();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_HandleRallyFeedback()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

        }
        #endregion

        #region Charge Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_RequestCharge()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            Rpc_HandleChargeFeedback();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_HandleChargeFeedback()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

        }
        #endregion

        #region Jump Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_RequestJump()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            _rigidbody.Rigidbody.AddForce(Vector3.up * _jumpVelocity, ForceMode.VelocityChange);

            Rpc_HandleJumpFeedback();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_HandleJumpFeedback()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

        }
        #endregion

        #region Aim Behaviour
        public void SetAim(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            _weaponController.SetAim(aimSource, aimDirection);
        }
        #endregion

        #region Death Behaviour
        public void SetToAlive()
        {
            if (!Runner.IsServer) return;

            _isAlive = true;
        }

        private void HandleDied_ServerOnly(LifeController controller)
        {
            if (!Runner.IsServer) return;

            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            _isAlive = false;
            yield return new WaitForSeconds(0.5f);
            _isAlive = true;
        }

        public static void HandleIsAliveChanged(Changed<EggChampionCharacter> changesHandler)
        {
            changesHandler.Behaviour._model.gameObject.SetActive(changesHandler.Behaviour._isAlive);
            changesHandler.Behaviour._collidersGO.SetActive(changesHandler.Behaviour._isAlive);
            changesHandler.Behaviour._rigidbody.Rigidbody.useGravity = changesHandler.Behaviour._isAlive;
        }
        #endregion
    }
}