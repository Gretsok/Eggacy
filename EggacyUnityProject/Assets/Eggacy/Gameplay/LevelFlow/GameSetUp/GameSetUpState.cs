using Eggacy.Gameplay.Combat.TeamManagement;
using Eggacy.Gameplay.LevelFlow.ChickenTankManagement;
using Eggacy.Gameplay.LevelFlow.TeamManagement;
using Eggacy.Gameplay.LevelFlow.WorldReferences;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.GameSetUp
{
    public class GameSetUpState : LevelFlowState
    {
        [SerializeField]
        private PlayerManagement.PlayerManager _playerManager = null;
        [SerializeField]
        private WorldReferencesHandler _worldReferencesHandler = null;
        [SerializeField]
        private TeamManager _teamManager = null;
        [SerializeField]
        private ChickenTankManager _chickenTankManger = null;

        protected override IEnumerator HandleServerSetUpRoutine()
        {
            yield return StartCoroutine(base.HandleServerSetUpRoutine());

            Debug.Log($"Player count : {(new List<Fusion.PlayerRef>(Runner.ActivePlayers)).Count}");

            _chickenTankManger.SetUp();

            yield return null;

            for (int i = 0; i < _chickenTankManger.tanksCount; ++i)
            {
                _chickenTankManger.SetTeamForTank(i, _teamManager.GetTeamDataByIndex(i));
            }

            yield return null;

            int numberOfPlayerCreated = 0;
            foreach (var player in Runner.ActivePlayers)
            {
                _playerManager.CreateControlsForPlayer(player);
                var character =  _playerManager.GetCharacterForPlayer(player);
                (var position, var rotation) = _worldReferencesHandler.GetSpawnPoint(numberOfPlayerCreated);
                character.networkRigidbody.TeleportToPositionRotation(position, rotation);
                var team = _teamManager.GetTeamDataByIndex(numberOfPlayerCreated);
                character.GetComponent<TeamController>().ChangeTeamData(team);

                var teamTank = _chickenTankManger.GetTankForTeam(team);
                
                character.SetRespawnPoint(teamTank.respawnPointsHandler.GetAndLockRespawnPoint());

                character.SetToAlive();

                ++numberOfPlayerCreated;
            }
            yield return null;


            onStateEnded?.Invoke(this);
        }
    }
}