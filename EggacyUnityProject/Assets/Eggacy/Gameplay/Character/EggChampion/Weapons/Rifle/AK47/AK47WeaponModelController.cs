using Eggacy.Gameplay.Combat.Weapon;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.Rifle.AK47
{
    public class AK47WeaponModelController : AWeaponModelController<AK47Weapon, AK47WeaponModel>
    {
        protected override void SetUp()
        {
            base.SetUp();
            weapon.onShoot += HandleShoot;
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            weapon.onShoot -= HandleShoot;
        }

        private void HandleShoot()
        {
            weaponModel.PlayShootFeedback();
        }
    }
}