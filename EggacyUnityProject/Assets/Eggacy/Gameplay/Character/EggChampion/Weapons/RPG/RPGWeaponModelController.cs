using Eggacy.Gameplay.Combat.Weapon;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG
{
    public class RPGWeaponModelController : AWeaponModelController<RPGWeapon, RPGWeaponModel>
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

        public override void Update()
        {
            base.Update();
            if (!weapon || !weaponModel) return;

            if(weapon.canShoot && !weaponModel.IsRocketModelActive)
            {
                weaponModel.ActivateRocketModel();
            }
            else if(!weapon.canShoot && weaponModel.IsRocketModelActive)
            {
                weaponModel.DeactivateRocketModel();
            }
        }
    }
}