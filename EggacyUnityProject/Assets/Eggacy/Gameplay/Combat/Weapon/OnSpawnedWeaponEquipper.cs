using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.Weapon
{
    public class OnSpawnedWeaponEquipper : NetworkBehaviour
    {
        [SerializeField]
        private HoldedWeaponController _weaponController = null;
        [SerializeField]
        private AWeapon _firstWeaponPrefab = null;
        public override void Spawned()
        {
            base.Spawned();

            if (!Runner.IsServer) return;

            _weaponController.ChangeWeapon(_firstWeaponPrefab);
        }
    }
}