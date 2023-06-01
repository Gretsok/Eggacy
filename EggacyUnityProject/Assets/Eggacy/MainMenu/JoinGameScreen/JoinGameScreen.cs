using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Eggacy.MainMenu
{
    public class JoinGameScreen : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _gameCodeInputField = null;
        [SerializeField]
        private Button _joinGameButton = null;
        [SerializeField]
        private Button _backButton = null;

        public Action onBackRequested = null;
        public Action<string> onJoinGameRequested = null;

        private void Awake()
        {
            _joinGameButton.onClick.AddListener(HandleJoinGameButtonClicked);
            _backButton.onClick.AddListener(HandleBackButtonClicked);
        }

        private void HandleBackButtonClicked()
        {
            onBackRequested?.Invoke();
        }

        private void HandleJoinGameButtonClicked()
        {
            onJoinGameRequested?.Invoke(_gameCodeInputField.text);
        }

        private void OnDestroy()
        {
            _joinGameButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
        }
    }
}