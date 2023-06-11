using Eggacy.Gameplay.Combat.Weapon;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.Rifle.AK47
{
    public class AK47WeaponModelController : AWeaponModelController<AK47Weapon, AK47WeaponModel>
    {
        [SerializeField]
        private ParticleSystem _shootFX = null;

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
            if(_shootFX)
                _shootFX.Play();
        }
    }
}