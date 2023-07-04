using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank
{
    public class ChickenTankCollider : MonoBehaviour
    {
        [SerializeField]
        private ChickenTank _chickenTank = null;
        public ChickenTank chickenTank => _chickenTank;
    }
}