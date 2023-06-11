using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Eggacy.Gameplay.Combat.TeamManagement
{
    public class TeamController : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(HandleTeamDataIndexChanged))]
        private int _teamDataIndex { get; set; }

        private TeamData _teamData = null;
        public TeamData teamData => _teamData;

        public void ChangeTeamData(TeamData teamData)
        {
            if (!Runner.IsServer) return;
            if(teamData == null)
            {
                Debug.LogError("Cannot set team to null");
                return;
            }

            _teamData = teamData;
            _teamDataIndex = _teamData.instanceIndex;
        }

        public static void HandleTeamDataIndexChanged(Changed<TeamController> changesHandler)
        {
            Addressables.LoadAssetsAsync<TeamData>("TeamData", (teamData =>
            {
                if(teamData != null && teamData.instanceIndex == changesHandler.Behaviour._teamDataIndex)
                {
                    changesHandler.Behaviour._teamData = teamData;
                }
            }));
        }
    }
}