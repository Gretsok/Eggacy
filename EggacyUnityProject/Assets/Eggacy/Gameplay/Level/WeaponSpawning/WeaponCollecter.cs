using Eggacy.Gameplay.Combat.Weapon;
using UnityEngine;

namespace Eggacy.Gameplay.Level.WeaponSpawner
{
    public class WeaponCollecter : MonoBehaviour
    {
        [SerializeField]
        private WeaponInventoryController _weaponInventoryController = null;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out WeaponSpawner spawner))
            {
                var weapon = spawner.TryToGetWeaponPickedUp();

                if(weapon)
                {
                    _weaponInventoryController.HoldAWeaponForALimitedNumberOfAttack(weapon.notCastedWeapon, 3);
                }
            }
        }
    }
}