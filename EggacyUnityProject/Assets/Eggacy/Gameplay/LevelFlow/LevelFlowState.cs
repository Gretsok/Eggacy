using Fusion;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Eggacy.Gameplay.LevelFlow
{
    public class LevelFlowState : NetworkBehaviour
    {
        [SerializeField]
        private UnityEvent onClientFeedbackTriggered = null;
        public Action<LevelFlowState> onStateEnded = null;

        public void Server_Enter()
        {
            if (!Runner.IsServer) return;

            StartCoroutine(SetUpRoutine());
        }

        private IEnumerator SetUpRoutine()
        {
            yield return StartCoroutine(HandleServerSetUpRoutine());

            Rpc_Client_Enter();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_Client_Enter()
        {
            if(!Runner.IsPlayer) return;

            HandleClientSetUp();
        }

        protected virtual IEnumerator HandleServerSetUpRoutine()
        {
            yield return null;
        }

        protected virtual void HandleClientSetUp()
        {
            onClientFeedbackTriggered?.Invoke();
        }

        public void Server_Update()
        {
            HandleServerUpdate();
        }

        protected virtual void HandleServerUpdate()
        {
        }

        public void Server_Leave()
        {
            HandleServerLeave();
        }

        protected virtual void HandleServerLeave()
        {

        }
    }
}