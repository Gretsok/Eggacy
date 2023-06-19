using Eggacy.Gameplay.Character.ChickenTank;
using Eggacy.Gameplay.Character.ChickenTank.Movement;
using Eggacy.Gameplay.Combat.LifeManagement;
using Eggacy.Gameplay.Combat.TeamManagement;
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
        public void InitializeForGameplay()
        {
            _chickenTanks.ForEach(chickenTank =>
            {
                chickenTank.lifeController.onDied_ServerOnly += HandleTankDied;
            });
        }

        public void CleanUpFromGameplay()
        {
            _chickenTanks.ForEach(chickenTank =>
            {
                chickenTank.lifeController.onDied_ServerOnly -= HandleTankDied;
            });
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

    }
}