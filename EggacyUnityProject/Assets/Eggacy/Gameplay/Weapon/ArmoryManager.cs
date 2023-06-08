
using UnityEngine;

namespace Eggacy.Gameplay.Weapon
{
    public class ArmoryManager : MonoBehaviour
    {
        [SerializeField]
        private WeaponsArmoryData _armoryData = null;
        public WeaponsArmoryData armoryData => _armoryData;
    }
}