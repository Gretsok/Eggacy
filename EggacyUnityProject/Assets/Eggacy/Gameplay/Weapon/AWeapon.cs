using Fusion;

namespace Eggacy.Gameplay.Weapon
{
    public class AWeapon : NetworkBehaviour
    {
        public void DoPrimaryAttack()
        {
            HandlePrimaryAttack();
        }

        protected virtual void HandlePrimaryAttack()
        { }

        public void DoSecondaryAttack()
        {
            HandleSecondaryAttack();
        }

        protected virtual void HandleSecondaryAttack()
        { }
    }
}