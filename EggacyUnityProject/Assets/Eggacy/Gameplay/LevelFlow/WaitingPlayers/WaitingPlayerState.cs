using Eggacy.Network;
using Fusion;
using System.Collections;
using System.Collections.Generic;

namespace Eggacy.Gameplay.LevelFlow.WaitingPlayers
{
    public class WaitingPlayerState : LevelFlowState
    {
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
            if(playersList.Count == 2)
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