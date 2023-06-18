using Eggacy.Gameplay.Combat.Damage;
using Eggacy.Gameplay.Combat.LifeManagement;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.Rifle.AK47
{
    public class AK47Weapon : ARifleWeapon
    {
        [SerializeField]
        private float _shootingRange = 100f;
        [SerializeField]
        private LayerMask _shootableMask = default;

        protected override void Shoot()
        {
            base.Shoot();
            if (Physics.Raycast(aimSource, aimDirection, out RaycastHit hitInfo, _shootingRange, _shootableMask))
            {
                if (hitInfo.collider.TryGetComponent(out LifeControllerCollider lifeControllerCollider))
                {
                    Damage damageToDeal = new Damage();
                    damageToDeal.amountToRetreat = damageAmount;
                    damageToDeal.teamSource = (referencesForWeaponContainer as EggChampionReferencesForWeaponContainer).teamController.teamData.team;
                    damageToDeal.source = (referencesForWeaponContainer as EggChampionReferencesForWeaponContainer).teamController.gameObject;
                    lifeControllerCollider.lifeController.TakeDamage(damageToDeal);
                }
            }
        }
    }
}