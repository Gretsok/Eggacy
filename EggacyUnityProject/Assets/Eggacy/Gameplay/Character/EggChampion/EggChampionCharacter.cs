using Eggacy.Gameplay.Combat.Weapon;
using Cinemachine.Utility;
using UnityEngine;
using Fusion;

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
        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (!_orientation.AlmostZero())
                _rigidbody.Rigidbody.rotation = Quaternion.LookRotation(_orientation, Vector3.up);

            float gravity = _rigidbody.Rigidbody.velocity.y;
            _currentPlannedVelocity = Vector3.Lerp(_currentPlannedVelocity, _directionToMove * _movementSpeed, Runner.DeltaTime * _movementSmoothness);
            _rigidbody.Rigidbody.velocity = _currentPlannedVelocity + gravity * Vector3.up;
        }

        public void SetDirectionToMove(Vector3 directionToMove)
        {
            _directionToMove = directionToMove;
        }

        public void SetOrientation(Vector3 orientation)
        {
            _orientation = orientation;
        }


        #region Attack Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StartPrimaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;

            if(_weaponController)
                _weaponController.StartPrimaryAttack(aimSource, aimDirection);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StartSecondaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;

            if (_weaponController)
                _weaponController.StartSecondaryAttack(aimSource, aimDirection);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StopPrimaryAttack()
        {
            if (!Runner.IsServer) return;

            if (_weaponController)
                _weaponController.StopPrimaryAttack();
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StopSecondaryAttack()
        {
            if (!Runner.IsServer) return;

            if (_weaponController)
                _weaponController.StopSecondaryAttack();
        }
        #endregion

        #region Rally Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_RequestRally()
        {
            if (!Runner.IsServer) return;

            Rpc_HandleRallyFeedback();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_HandleRallyFeedback()
        {

        }
        #endregion

        #region Charge Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_RequestCharge()
        {
            if (!Runner.IsServer) return;

            Rpc_HandleChargeFeedback();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_HandleChargeFeedback()
        {

        }
        #endregion

        #region Jump Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_RequestJump()
        {
            if (!Runner.IsServer) return;


            _rigidbody.Rigidbody.AddForce(Vector3.up * _jumpVelocity, ForceMode.VelocityChange);

            Rpc_HandleJumpFeedback();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_HandleJumpFeedback()
        {

        }
        #endregion

        #region Aim Behaviour
        public void SetAim(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;

            _weaponController.SetAim(aimSource, aimDirection);
        }
        #endregion
    }
}