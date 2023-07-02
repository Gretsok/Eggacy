using Eggacy.Gameplay.Combat.Weapon;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.Rifle.AK47
{
    public class AK47WeaponModelController : AWeaponModelController<AK47Weapon, AK47WeaponModel>
    {
        private void Start()
        {
            weapon.onShoot += HandleShoot;
        }

        private void OnDestroy()
        {
            weapon.onShoot -= HandleShoot;
        }

        private void HandleShoot()
        {
            weaponModel.PlayShootFeedback();
        }
    }
}