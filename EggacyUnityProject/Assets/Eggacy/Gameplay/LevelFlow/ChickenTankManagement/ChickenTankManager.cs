using Eggacy.Gameplay.Character.ChickenTank;
using Eggacy.Gameplay.Character.ChickenTank.Movement;
using Eggacy.Gameplay.Combat.LifeManagement;
using Eggacy.Gameplay.Combat.TeamManagement;
using Eggacy.Gameplay.LevelFlow.TeamManagement;
using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.ChickenTankManagement
{
    public class ChickenTankManager : MonoBehaviour
    {
        [SerializeField]
        private ChickenWaypointsManager _waypointsManager = null;
        [SerializeField]
        private List<ChickenTank> _chickenTanks = new List<ChickenTank>();
        public int tanksCount => _chickenTanks.Count;
        [SerializeField]
        private List<ChickenWaypoint> _startingChickenWaypoints = new List<ChickenWaypoint>();

        public Action<ChickenTank> onTankDestroyed = null;


        public Action<ChickenTankManager> onInitializedForGameplay = null;
        public Action<ChickenTankManager> onCleanedUpFromGameplay = null;


        public void InitializeForGameplay()
        {
            _chickenTanks.ForEach(chickenTank =>
            {
                chickenTank.lifeController.onDied_ServerOnly += HandleTankDied;
            });
            Rpc_NotifyInitializedForGameplay();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyInitializedForGameplay()
        {
            onInitializedForGameplay?.Invoke(this);
        }

        public void CleanUpFromGameplay()
        {
            _chickenTanks.ForEach(chickenTank =>
            {
                chickenTank.lifeController.onDied_ServerOnly -= HandleTankDied;
            });
            Rpc_NotifyCleanedUpFromGameplay();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyCleanedUpFromGameplay()
        {
            onCleanedUpFromGameplay?.Invoke(this);
        }

        private void HandleTankDied(LifeController lifeController)
        {
            var tank = _chickenTanks.Find(c => c.lifeController == lifeController);
            if (!tank) return;

            onTankDestroyed?.Invoke(tank);
        }


        public void SetUp()
        {
            for (int i = 0; i < _chickenTanks.Count; ++i)
            {
                _chickenTanks[i].movementController.SetWaypointsManager(_waypointsManager);
                _chickenTanks[i].movementController.SetStartingWaypoint(_startingChickenWaypoints[i]);
                _chickenTanks[i].movementController.SetReady();
            }
        }

        public void SetTeamForTank(int teamIndex, TeamData teamData)
        {
            _chickenTanks[teamIndex].GetComponent<TeamController>().ChangeTeamData(teamData);
        }

        public ChickenTank GetTankAt(int index)
        {
            return _chickenTanks[index];
        }

        public ChickenTank GetTankForTeam(TeamData teamData)
        {
            return _chickenTanks.Find(tank => tank.lifeController.teamController.teamData.instanceIndex == teamData.instanceIndex);
        }
    }
}