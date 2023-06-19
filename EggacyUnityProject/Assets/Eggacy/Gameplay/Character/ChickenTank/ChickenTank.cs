using Eggacy.Gameplay.Character.ChickenTank.Movement;
using Eggacy.Gameplay.Combat.LifeManagement;
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
    }
}