using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.TeamManagement
{
    [System.Serializable]
    public class Team
    {
        [SerializeField]
        private string _teamName = "Alpha";
        public string teamName => _teamName;

        [SerializeField]
        private Color _teamColor = Color.blue;
        public Color teamColor => _teamColor;

        [SerializeField]
        private bool _friendlyFire = false;
        public bool friendlyFire => _friendlyFire;

        [SerializeField]
        private List<TeamData> _alliedTeams = new List<TeamData>();
        public int alliedTeamsCount => _alliedTeams.Count;
        public bool IsAlliedWith(TeamData team)
        {
            return _alliedTeams.Contains(team);
        }
        public TeamData GetAlliedTeamAt(int index)
        {
            if (index < 0 || index >= alliedTeamsCount)
                return null;
            return _alliedTeams[index];
        }
    }
}