using System;
using UnityEngine;
using UnityEngine.UI;

namespace Eggacy.MainMenu
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField]
        private Button _playButton = null;
        [SerializeField]
        private Button _quitButton = null;

        public Action onQuitGameRequested = null;
        public Action onJoinGameScreenRequested;

        private void Awake()
        {
            _playButton.onClick.AddListener(HandlePlayButtonClicked);
            _quitButton.onClick.AddListener(HandleQuitButtonClicked);
        }

        private void HandleQuitButtonClicked()
        {
            onQuitGameRequested?.Invoke();
        }

        private void HandlePlayButtonClicked()
        {
            onJoinGameScreenRequested?.Invoke();
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }
    }
}