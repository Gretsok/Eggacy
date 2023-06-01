using Eggacy.Network;
using UnityEngine;

namespace Eggacy.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private NetworkManager _networkManager = null;
        [SerializeField]
        private MainMenuUIController _mainMenuUIController = null;

        private void Awake()
        {
            _mainMenuUIController.onJoinGameRequested += HandleJoinGameRequested;
            _mainMenuUIController.onQuitGameRequested += HandleQuitGameRequested;
        }

        private void HandleQuitGameRequested()
        {
            QuitGame();
        }

        private void HandleJoinGameRequested(string roomName)
        {
            StartGame(roomName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void StartGame(string roomName)
        {
            _networkManager.StartGame(roomName);
        }

        private void OnDestroy()
        {
            _mainMenuUIController.onJoinGameRequested -= HandleJoinGameRequested;
            _mainMenuUIController.onQuitGameRequested -= HandleQuitGameRequested;
        }
    }
}