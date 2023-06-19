using Eggacy.Gameplay.LevelFlow.UIManagement;
using System;
using System.Collections;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.Result
{
    public class ResultScreen : FlowUIScreen
    {
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
            StartCoroutine(WaitForWinningTeamDataInfo(DisplayContainerAccordingToGameScore));
        }

        private void DisplayContainerAccordingToGameScore()
        {
            if (_teamManager.winningTeamData == _playerManager.localChampionCharacter.lifeController.teamController.teamData)
            {
                _winContainer.SetActive(true);
                _loseContainer.SetActive(false);
            }
            else
            {
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