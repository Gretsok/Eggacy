using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Weapon
{
    /// <summary>
    /// This class only manage the weapon currently holded. Not the weapons inventory.
    /// </summary>
    public class HoldedWeaponController : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(HandleCurrentWeaponIndexEquippedChanged))]
        private int _currentWeaponIndexEquipped { get; set; }
        private AWeapon _weaponController = null;

        [SerializeField]
        private Transform _weaponContainer = null;

        public void ChangeWeapon(AWeapon weaponControllerPrefab)
        {
            if (!Runner.IsServer) return;

            Runner.Spawn(weaponControllerPrefab, _weaponContainer.position, _weaponContainer.rotation, inputAuthority: Runner.LocalPlayer, InitializeWeaponBeforeSynchronisation, predictionKey: null);
        }

        private void InitializeWeaponBeforeSynchronisation(NetworkRunner runner, NetworkObject obj)
        {
            obj.transform.SetParent(_weaponContainer, false);
            obj.transform.localPosition = default;
            obj.transform.localRotation = default;

            if(obj.TryGetComponent(out AWeapon weapon))
            {
                _weaponController = weapon;
            }
        }

        public void DoPrimaryAttack()
        {
            if (!Runner.IsServer) return;

            if (_weaponController)
                _weaponController.DoPrimaryAttack();
            
        }

        public void DoSecondaryAttack()
        {
            if (!Runner.IsServer) return;

            if (_weaponController)
                _weaponController.DoSecondaryAttack();

        }

        public static void HandleCurrentWeaponIndexEquippedChanged(Changed<HoldedWeaponController> changesHandler)
        {

        }

    }
}