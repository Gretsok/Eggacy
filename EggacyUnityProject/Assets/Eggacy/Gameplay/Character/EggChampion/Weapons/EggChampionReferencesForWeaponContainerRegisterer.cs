using Eggacy.Gameplay.Combat.Weapon;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons
{
    public class EggChampionReferencesForWeaponContainerRegisterer : MonoBehaviour
    {
        [SerializeField]
        private EggChampionReferencesForWeaponContainer _referencesForWeaponContainer = null;
        [SerializeField]
        private HoldedWeaponController _holdedWeaponController = null;

        private void Start()
        {
            _holdedWeaponController.SetReferencesForWeaponContainer(_referencesForWeaponContainer);
        }
    }
}