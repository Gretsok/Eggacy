using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.WorldReferences
{
    public class WorldReferencesHandler : MonoBehaviour
    {
        [System.Serializable]
        public struct STeamWorldReferences
        {
            public Transform PlayerSpawnPoint;
        }

        [SerializeField]
        private List<STeamWorldReferences> _teamWorldReferences = null;
        
        public (Vector3, Quaternion) GetSpawnPoint(int teamIndex)
        {
            var teamWorldRefs = _teamWorldReferences[teamIndex % _teamWorldReferences.Count];
            return (teamWorldRefs.PlayerSpawnPoint.position, teamWorldRefs.PlayerSpawnPoint.rotation);
        }
    }
}