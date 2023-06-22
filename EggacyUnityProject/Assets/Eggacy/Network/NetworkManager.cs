using Eggacy.Gameplay.Character.EggChampion;
using Eggacy.Gameplay.Character.EggChampion.Player;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Eggacy.Network
{
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        private NetworkRunner _runner = null;

        private EggChampionPlayerController _localPlayerController = null;

        public Action<PlayerRef> onNewPlayerJoined = null;
        public Action<PlayerRef> onPlayerLeft = null;

        public void SetLocalPlayerController(EggChampionPlayerController localPlayerController)
        {
            _localPlayerController = localPlayerController;
        }

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
            if(_localPlayerController)
            {
                input.Set(_localPlayerController.networkedInput);
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
            onNewPlayerJoined?.Invoke(player);

            if (!_runner.IsServer) return;


        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            onPlayerLeft?.Invoke(player);
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
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
                PlayerCount = 10
            });
        }

        private void OnDestroy()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}