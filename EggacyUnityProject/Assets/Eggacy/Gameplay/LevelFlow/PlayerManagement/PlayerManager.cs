using Eggacy.Gameplay.Character.EggChampion.Player;
using Eggacy.Gameplay.Character.EggChampion;
using Fusion;
using System.Collections.Generic;
using UnityEngine;
using Eggacy.Gameplay.Combat.TeamManagement;

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

        private EggChampionCharacter _localChampionCharacter = null;
        public EggChampionCharacter localChampionCharacter => _localChampionCharacter;

        private EggChampionCharacter _winningChampionCharacter = null;
        public EggChampionCharacter winningChampionCharacter => _winningChampionCharacter;

        public void CreateControlsForPlayer(PlayerRef player)
        {
            EggChampionCharacter networkedCharacter = Runner.Spawn(_characterPrefab, default, Quaternion.identity, player)
                .GetComponent<EggChampionCharacter>();
            _characters.Add(player, networkedCharacter);

            EggChampionPlayerController networkedPlayerController = Runner.Spawn(_playerControllerPrefab, default, Quaternion.identity, player)
                .GetComponent<EggChampionPlayerController>();
            _playerControllers.Add(player, networkedPlayerController);

            networkedPlayerController.SetCharacter(networkedCharacter);

            Rpc_NotifyNewCharacterCreated(networkedCharacter);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyNewCharacterCreated(EggChampionCharacter newCreatedCharacter)
        {
            if(newCreatedCharacter.HasInputAuthority)
            {
                _localChampionCharacter = newCreatedCharacter;
            }
        }

        public EggChampionCharacter GetCharacterForPlayer(PlayerRef player)
        {
            return _characters.ContainsKey(player) ? _characters[player] : null;
        }
    }
}