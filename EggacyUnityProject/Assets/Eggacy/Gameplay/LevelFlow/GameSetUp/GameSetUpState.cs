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

        protected override IEnumerator HandleServerSetUpRoutine()
        {
            yield return StartCoroutine(base.HandleServerSetUpRoutine());

            Debug.Log($"Player count : {(new List<Fusion.PlayerRef>(Runner.ActivePlayers)).Count}");

            int numberOfPlayerCreated = 0;
            foreach (var player in Runner.ActivePlayers)
            {
                _playerManager.CreateControlsForPlayer(player);
                var character =  _playerManager.GetCharacterForPlayer(player);
                (var position, var rotation) = _worldReferencesHandler.GetSpawnPoint(numberOfPlayerCreated % 2);
                character.networkRigidbody.TeleportToPositionRotation(position, rotation);

                ++numberOfPlayerCreated;
            }
            yield return null;
            onStateEnded?.Invoke(this);
        }
    }
}