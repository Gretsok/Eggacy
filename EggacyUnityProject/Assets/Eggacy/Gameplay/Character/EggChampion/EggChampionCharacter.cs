using Cinemachine.Utility;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion
{
    public class EggChampionCharacter : NetworkBehaviour
    {
        [SerializeField]
        private NetworkRigidbody _rigidbody = null;
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
            if(!_orientation.AlmostZero())
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
            Rpc_HandleOrientationSetUp(orientation);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Channel = RpcChannel.Unreliable)]
        public void Rpc_HandleOrientationSetUp(Vector3 orientation)
        {
            _orientation = orientation;
        }

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
    }
}