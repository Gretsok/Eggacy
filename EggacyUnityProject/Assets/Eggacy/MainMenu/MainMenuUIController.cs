using System;
using UnityEngine;

namespace Eggacy.MainMenu
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField]
        private MainScreen _mainScreen = null;
        [SerializeField]
        private JoinGameScreen _joinGameScreen = null;

        public Action onQuitGameRequested = null;
        public Action<string> onJoinGameRequested = null;

        private void Awake()
        {
            _mainScreen.onQuitGameRequested += HandleQuitGameRequested;
            _mainScreen.onJoinGameScreenRequested += HandleJoinGameScreenRequested;
            _joinGameScreen.onBackRequested += HandleBackFromJoinGameScreenRequested;
            _joinGameScreen.onJoinGameRequested += HandleJoinGameRequested;

            _mainScreen.gameObject.SetActive(true);
            _joinGameScreen.gameObject.SetActive(false);
        }

        private void HandleBackFromJoinGameScreenRequested()
        {
            _mainScreen.gameObject.SetActive(true);
            _joinGameScreen.gameObject.SetActive(false);
        }

        private void HandleJoinGameRequested(string roomName)
        {
            onJoinGameRequested?.Invoke(roomName);
        }

        private void HandleJoinGameScreenRequested()
        {
            _mainScreen.gameObject.SetActive(false);
            _joinGameScreen.gameObject.SetActive(true);
        }

        private void HandleQuitGameRequested()
        {
            onQuitGameRequested?.Invoke();
        }

        private void OnDestroy()
        {
            _mainScreen.onQuitGameRequested -= HandleQuitGameRequested;
            _mainScreen.onJoinGameScreenRequested -= HandleJoinGameScreenRequested;
            _joinGameScreen.onBackRequested -= HandleBackFromJoinGameScreenRequested;
            _joinGameScreen.onJoinGameRequested -= HandleJoinGameRequested;
        }
    }
}