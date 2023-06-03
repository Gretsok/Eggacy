using Eggacy.Gameplay.Character.EggChampion.Player;
using Eggacy.Gameplay.Character.EggChampion;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.PlayerManagement
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField]
        private NetworkPrefabRef _characterPrefab = default;
        [SerializeField]
        private NetworkPrefabRef _playerControllerPrefab = default;

        private Dictionary<PlayerRef, EggChampionCharacter> _characters = new Dictionary<PlayerRef, EggChampionCharacter>();
        private Dictionary<PlayerRef, EggChampionPlayerController> _playerControllers = new Dictionary<PlayerRef, EggChampionPlayerController>();

        public IEnumerable<EggChampionCharacter> characters => _characters.Values;

        public void CreateControlsForPlayer(PlayerRef player)
        {
            EggChampionCharacter networkedCharacter = Runner.Spawn(_characterPrefab, default, Quaternion.identity, player)
                .GetComponent<EggChampionCharacter>();
            _characters.Add(player, networkedCharacter);

            EggChampionPlayerController networkedPlayerController = Runner.Spawn(_playerControllerPrefab, default, Quaternion.identity, player)
                .GetComponent<EggChampionPlayerController>();
            _playerControllers.Add(player, networkedPlayerController);

            networkedPlayerController.SetCharacter(networkedCharacter);
        }

        public EggChampionCharacter GetCharacterForPlayer(PlayerRef player)
        {
            return _characters.ContainsKey(player) ? _characters[player] : null;
        }
    }
}