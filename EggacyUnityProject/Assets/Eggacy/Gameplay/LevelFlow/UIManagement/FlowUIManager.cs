using Eggacy.Gameplay.LevelFlow.Game;
using Eggacy.Gameplay.LevelFlow.WaitingPlayers;
using Fusion;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.UIManagement
{
    public class FlowUIManager : MonoBehaviour
    {
        [SerializeField]
        private LevelFlowManager _levelFlowManager = null;

        [SerializeField]
        private WaitingPlayersScreen _waitingScreen = null;
        [SerializeField]
        private GameScreen _gameScreen = null;

        private void Start()
        {
            _levelFlowManager.onStateUpdated += HandleStateUpdated;
            SetUpUI(0);
        }

        private void OnDestroy()
        {
            _levelFlowManager.onStateUpdated -= HandleStateUpdated;
        }

        private void HandleStateUpdated(LevelFlowManager levelFlowManager)
        {
            SetUpUI(levelFlowManager.currentStateIndex);
        }

        public void HideEverything()
        {
            _waitingScreen.Disable();
            _gameScreen.Disable();
        }

        public void ShowWaitingScreen()
        {
            HideEverything();
            _waitingScreen.Enable();
        }

        public void ShowGameScreen()
        {
            HideEverything();
            _gameScreen.Enable();
        }

        private void SetUpUI(int stateIndex)
        {
            switch(stateIndex)
            {
                case 0:
                    ShowWaitingScreen();
                    break;
                case 1:
                    HideEverything();
                    break;
                case 2:
                    ShowGameScreen();
                    break;
                case 3:
                    HideEverything();
                    break;
                default:
                    HideEverything();
                    break;
            }
        }
    }
}