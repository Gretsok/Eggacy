using Eggacy.Gameplay.Combat.TeamManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.GameSetUp
{
    public class TeamHandlerForGameSetUp : MonoBehaviour
    {
        [SerializeField]
        private List<TeamData> _teamDataPool = new List<TeamData>();

        [SerializeField]
        private int _numberOfTeams = 2;

        private List<TeamData> _orderedTeamDataPool = null;

        private void Start()
        {
            OrderTeamDataPool();
        }

        private void OrderTeamDataPool()
        {
            if (_numberOfTeams > _teamDataPool.Count)
            {
                Debug.LogError("Not enough team data in pool");
                return;
            }

            _orderedTeamDataPool = new List<TeamData>();

            List<int> randomIndexes = new List<int>();
            do
            {
                var randomIndex = Random.Range(0, _teamDataPool.Count);
                if(!randomIndexes.Contains(randomIndex))
                {
                    randomIndexes.Add(randomIndex);
                }

            }while (randomIndexes.Count < _numberOfTeams);

            for(int i = 0; i < _numberOfTeams; ++i)
            {
                _orderedTeamDataPool.Add(_teamDataPool[randomIndexes[i]]);
            }
        }

        public TeamData GetTeamDataByIndex(int index)
        {
            Debug.Log($"Get team {_orderedTeamDataPool[index % _orderedTeamDataPool.Count].team.teamName}");
            return _orderedTeamDataPool[index % _orderedTeamDataPool.Count];
        }
    }
}