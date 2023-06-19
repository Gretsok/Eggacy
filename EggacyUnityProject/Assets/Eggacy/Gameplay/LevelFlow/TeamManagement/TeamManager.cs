using Eggacy.Gameplay.Combat.TeamManagement;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.TeamManagement
{
    public class TeamManager : NetworkBehaviour
    {
        [SerializeField]
        private List<TeamData> _teamDataPool = new List<TeamData>();
        private List<TeamData> _livingTeamData = new List<TeamData>();
        [SerializeField]
        private int _numberOfTeams = 2;

        private List<TeamData> _orderedTeamDataPool = null;



        private TeamData _winningTeamData = null;
        public TeamData winningTeamData => _winningTeamData;

        private void Start()
        {
            OrderTeamDataPool();
            _livingTeamData = new List<TeamData>(_teamDataPool);
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
                if (!randomIndexes.Contains(randomIndex))
                {
                    randomIndexes.Add(randomIndex);
                }

            } while (randomIndexes.Count < _numberOfTeams);

            for (int i = 0; i < _numberOfTeams; ++i)
            {
                _orderedTeamDataPool.Add(_teamDataPool[randomIndexes[i]]);
            }
        }

        public TeamData GetTeamDataByIndex(int index)
        {
            Debug.Log($"Get team {_orderedTeamDataPool[index % _orderedTeamDataPool.Count].team.teamName}");
            return _orderedTeamDataPool[index % _orderedTeamDataPool.Count];
        }

        public void SetLosingTeamData(TeamData losingTeamData)
        {

            _livingTeamData.RemoveAll(teamData => teamData.instanceIndex == losingTeamData.instanceIndex);
            var livingTeamLeft = _livingTeamData.Count - (_orderedTeamDataPool.Count - _numberOfTeams);

            Debug.LogError($"Adding losing team | left : {livingTeamLeft}");
            if(livingTeamLeft <=  1)
            {
                Debug.LogError("Setting winner");
                _winningTeamData = _livingTeamData[0];
                Rpc_SettingWinningTeamData(winningTeamData.instanceIndex);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_SettingWinningTeamData(int indexWinningTeamData)
        {
            Debug.LogError("Setting up winning team data"); 
            _winningTeamData = _teamDataPool.Find(data => data.instanceIndex == indexWinningTeamData); 
        }
    }
}