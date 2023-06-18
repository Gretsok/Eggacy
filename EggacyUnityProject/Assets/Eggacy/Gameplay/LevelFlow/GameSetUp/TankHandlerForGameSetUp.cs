using Eggacy.Gameplay.Character.ChickenTank;
using Eggacy.Gameplay.Character.ChickenTank.Movement;
using Eggacy.Gameplay.Combat.TeamManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.GameSetUp
{
    public class TankHandlerForGameSetUp : MonoBehaviour
    {
        [SerializeField]
        private ChickenWaypointsManager _waypointsManager = null;
        [SerializeField]
        private List<ChickenTank> _tanks = new List<ChickenTank>();
        [SerializeField]
        private List<ChickenWaypoint> _startingChickenWaypoints = new List<ChickenWaypoint>();

        public void SetUp()
        {
            for(int i = 0; i < _tanks.Count; ++i)
            {
                _tanks[i].movementController.SetWaypointsManager(_waypointsManager);
                _tanks[i].movementController.SetStartingWaypoint(_startingChickenWaypoints[i]);
                _tanks[i].movementController.SetReady();
            }
        }

        public void SetTeamForTank(int teamIndex, TeamData teamData)
        {

        }


    }
}