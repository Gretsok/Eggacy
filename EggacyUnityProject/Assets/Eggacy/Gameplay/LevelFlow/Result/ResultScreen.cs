using Eggacy.Gameplay.LevelFlow.UIManagement;
using System;
using System.Collections;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.Result
{
    public class ResultScreen : FlowUIScreen
    {
        [SerializeField]
        private GameObject _waitingForDataContainer = null;
        [SerializeField]
        private GameObject _winContainer = null;
        [SerializeField]
        private GameObject _loseContainer = null;

        [SerializeField]
        private PlayerManagement.PlayerManager _playerManager = null;
        [SerializeField]
        private TeamManagement.TeamManager _teamManager = null;

        protected override void SetUp()
        {
            base.SetUp();
            _waitingForDataContainer.SetActive(true);
            _winContainer.SetActive(false);
            _loseContainer.SetActive(false);
            StartCoroutine(WaitForWinningTeamDataInfo(DisplayContainerAccordingToGameScore));
        }

        private void DisplayContainerAccordingToGameScore()
        {
            if (_teamManager.winningTeamData.instanceIndex == _playerManager.localChampionCharacter.lifeController.teamController.teamData.instanceIndex)
            {
                _waitingForDataContainer.SetActive(false);
                _winContainer.SetActive(true);
                _loseContainer.SetActive(false);
            }
            else
            {
                _waitingForDataContainer.SetActive(false);
                _winContainer.SetActive(false);
                _loseContainer.SetActive(true);
            }
        }

        private IEnumerator WaitForWinningTeamDataInfo(Action winningTeamDataReceivedCallback)
        {
            while(_teamManager.winningTeamData == null)
            {
                yield return null;
            }

            winningTeamDataReceivedCallback();
        }
    }
}