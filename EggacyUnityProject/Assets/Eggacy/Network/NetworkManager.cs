using Eggacy.Gameplay.Character.EggChampion;
using Eggacy.Gameplay.Character.EggChampion.Player;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Network
{
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        private NetworkRunner _runner = null;

        [SerializeField]
        private NetworkPrefabRef _characterPrefab = default;
        [SerializeField]
        private NetworkPrefabRef _playerControllerPrefab = default;

        private Dictionary<PlayerRef, EggChampionCharacter> _characters = new Dictionary<PlayerRef, EggChampionCharacter>();
        private Dictionary<PlayerRef, EggChampionPlayerController> _playerControllers = new Dictionary<PlayerRef, EggChampionPlayerController>();

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            if(_playerControllers.ContainsKey(runner.LocalPlayer))
            {
                input.Set(_playerControllers[runner.LocalPlayer].networkedInput);
            }
            else
            {
                input.Set(new NetworkedInput());
            }
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (!_runner.IsServer) return;

            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            EggChampionCharacter networkedCharacter = runner.Spawn(_characterPrefab, spawnPosition, Quaternion.identity, player)
                .GetComponent<EggChampionCharacter>();
            _characters.Add(player, networkedCharacter);

            EggChampionPlayerController networkedPlayerController = runner.Spawn(_playerControllerPrefab, spawnPosition, Quaternion.identity, player)
                .GetComponent<EggChampionPlayerController>();
            _playerControllers.Add(player, networkedPlayerController);

            networkedPlayerController.SetCharacter(networkedCharacter);
            networkedPlayerController.Initialize();
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            Debug.Log("Scene Loaded");
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public async void StartGame(string roomName)
        { 
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = roomName,
                Scene = 1,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
    }
}