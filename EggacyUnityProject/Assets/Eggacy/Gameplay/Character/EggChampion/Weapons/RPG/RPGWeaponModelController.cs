using Eggacy.Gameplay.Combat.Weapon;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG
{
    public class RPGWeaponModelController : AWeaponModelController<RPGWeapon, RPGWeaponModel>
    {

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