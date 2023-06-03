using Eggacy.Gameplay.LevelFlow.Game;
using Eggacy.Gameplay.LevelFlow.WaitingPlayers;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.UIManagement
{
    public class FlowUIManager : MonoBehaviour
    {
        [SerializeField]
        private WaitingPlayersScreen _waitingScreen = null;
        [SerializeField]
        private GameScreen _gameScreen = null;

        public void HideEverything()
        {
            _waitingScreen.Disable();
            _gameScreen.Disable();
        }

        public void ShowWaitingScreen()
        {
            Debug.Log("Waiting");
            HideEverything();
            _waitingScreen.Enable();
        }

        public void ShowGameScreen()
        {
            HideEverything();
            _gameScreen.Enable();
        }
    }
}