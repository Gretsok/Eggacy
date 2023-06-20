using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank.RespawnPointsHandler
{
    public class ChickenTankRespawnPointsHandler : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> _respawnPoints = null;
        private List<Transform> _lockedRespawnPoints = new List<Transform>();

        public Transform GetAndLockRespawnPoint()
        {
            var respawnPoint = _respawnPoints.Find(rp => !_lockedRespawnPoints.Contains(rp));
            _lockedRespawnPoints.Add(respawnPoint); 
            return respawnPoint;
        }

        public void UnlockAllRespawnPoints()
        {
            _lockedRespawnPoints.Clear();
        }
    }
}