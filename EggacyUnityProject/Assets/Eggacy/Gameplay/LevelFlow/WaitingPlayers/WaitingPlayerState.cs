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

        protected override IEnumerator HandleServerSetUpRoutine()
        {
            yield return StartCoroutine(base.HandleServerSetUpRoutine());

            _networkManager = Runner.GetComponent<NetworkManager>();
            _networkManager.onNewPlayerJoined += HandleNewPlayerJoined;

            TryToStartGameIfConditionsFullfilled();
        }

        private void HandleNewPlayerJoined(PlayerRef obj)
        {
            TryToStartGameIfConditionsFullfilled();
        }

        private void TryToStartGameIfConditionsFullfilled()
        {
            var playersList = new List<PlayerRef>(Runner.ActivePlayers);
            if(playersList.Count == _playerCount)
            {
                Runner.SessionInfo.IsOpen = false;
                RequestStartGame();
            }
        }

        private void RequestStartGame()
        {
            _networkManager.onNewPlayerJoined -= HandleNewPlayerJoined;
            onStateEnded?.Invoke(this);
        }
    }
}