using Eggacy.Gameplay.Character.ChickenTank.Movement;
using Eggacy.Gameplay.Character.ChickenTank.RespawnPointsHandler;
using Eggacy.Gameplay.Combat.LifeManagement;
using Eggacy.Gameplay.Combat.TeamManagement;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank
{
    public class ChickenTank : NetworkBehaviour
    {
        [SerializeField]
        private ChickenMovementController _movementController = null;
        public ChickenMovementController movementController => _movementController;
        [SerializeField]
        private LifeController _lifeController = null;
        public LifeController lifeController => _lifeController;
        [SerializeField]
        private ChickenTankRespawnPointsHandler _respawnPointsHandler = null;
        public ChickenTankRespawnPointsHandler respawnPointsHandler => _respawnPointsHandler;
        [SerializeField]
        private TeamController _teamController = null;
        public TeamController teamController => _teamController;
    }
}