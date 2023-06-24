using Eggacy.Gameplay.Character.EggChampion.Player;
using Eggacy.Gameplay.Character.EggChampion;
using Fusion;
using System.Collections.Generic;
using UnityEngine;
using Eggacy.Gameplay.Combat.TeamManagement;
using System.Collections;

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

            Rpc_NotifyNewCharacterCreated(networkedCharacter.Object.Id);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_NotifyNewCharacterCreated(NetworkId newCreatedCharacterIndex)
        {
            StartCoroutine(HandleLocalCharacterSetUp(newCreatedCharacterIndex));
        }

        private IEnumerator HandleLocalCharacterSetUp(NetworkId newCreatedCharacterIndex)
        {
            NetworkObject characterObject = Runner.FindObject(newCreatedCharacterIndex);

            while(characterObject == null)
            {
                yield return null;
                characterObject = Runner.FindObject(newCreatedCharacterIndex);
            }

            if (characterObject.HasInputAuthority)
            {
                _localChampionCharacter = characterObject.GetComponent<EggChampionCharacter>();
            }
        }

        public EggChampionCharacter GetCharacterForPlayer(PlayerRef player)
        {
            return _characters.ContainsKey(player) ? _characters[player] : null;
        }
    }
}