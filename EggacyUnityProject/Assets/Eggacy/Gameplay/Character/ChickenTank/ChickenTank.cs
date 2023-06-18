using Eggacy.Gameplay.Character.ChickenTank.Movement;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank
{
    public class ChickenTank : NetworkBehaviour
    {
        [SerializeField]
        private ChickenMovementController _movementController = null;
        public ChickenMovementController movementController => _movementController;
    }
}