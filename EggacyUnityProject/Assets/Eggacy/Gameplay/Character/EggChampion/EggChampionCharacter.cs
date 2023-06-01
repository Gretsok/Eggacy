using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion
{
    public class EggChampionCharacter : NetworkBehaviour
    {
        private Vector3 _directionToMove = default;
        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

        }

        public void SetDirectionToMove(Vector3 directionToMove)
        {
            _directionToMove = directionToMove;
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
    }
}