using Eggacy.Network;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.WaitingPlayers
{
    public class WaitingPlayerState : LevelFlowState
    {
        [SerializeField]
        private int _playerCount = 2;
        private NetworkManager _networkManager = null;
        private List<PlayerRef> _loadedPlayers = new List<PlayerRef>();

        public override void Spawned()
        {
            base.Spawned();
            _networkManager = Runner.GetComponent<NetworkManager>();
            Rpc_NotifyPlayerLoaded(Runner.LocalPlayer);
        }

        private void Start()
        {
            if (!Runner) return;
            if (!Runner.IsRunning) return;

            Rpc_NotifyPlayerLoaded(Runner.LocalPlayer);
        }

        protected override IEnumerator HandleServerSetUpRoutine()
        {
            yield return StartCoroutine(base.HandleServerSetUpRoutine());

            _networkManager = Runner.GetComponent<NetworkManager>();
            _networkManager.onNewPlayerJoined += HandleNewPlayerJoined;
            _networkManager.onPlayerLeft += HandlePlayerLeft;

            TryToStartGameIfConditionsFullfilled();
        }

        private void HandlePlayerLeft(PlayerRef player)
        {
            _loadedPlayers.RemoveAll(x => x == player);
        }

        private void HandleNewPlayerJoined(PlayerRef obj)
        {
            // TryToStartGameIfConditionsFullfilled();
        }

        private void TryToStartGameIfConditionsFullfilled()
        {
            var playersList = new List<PlayerRef>(Runner.ActivePlayers);
            if(playersList.Count == _playerCount && _loadedPlayers.Count == _playerCount)
            {
                Runner.SessionInfo.IsOpen = false;
                RequestStartGame();
            }
        }

        private void RequestStartGame()
        {
            _networkManager.onNewPlayerJoined -= HandleNewPlayerJoined;
            _networkManager.onPlayerLeft -= HandlePlayerLeft;
            onStateEnded?.Invoke(this);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void Rpc_NotifyPlayerLoaded(PlayerRef player)
        {
            if(!_loadedPlayers.Find(p => p == player).IsValid)
                _loadedPlayers.Add(player);
            TryToStartGameIfConditionsFullfilled();
        }
    }
}