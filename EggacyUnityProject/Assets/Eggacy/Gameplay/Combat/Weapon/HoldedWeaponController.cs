using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.Weapon
{
    /// <summary>
    /// This class only manage the weapon currently holded. Not the weapons inventory.
    /// </summary>
    public class HoldedWeaponController : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(HandleCurrentWeaponIndexEquippedChanged))]
        private NetworkBehaviourId _currentWeaponIndexEquipped { get; set; }
        private AWeapon _currentWeapon = null;
        public AWeapon currentWeapon => _currentWeapon;

        [SerializeField]
        private Transform _weaponContainer = null;

        private Vector3 _aimSource = default, _aimDirection = default;
        
        private bool _primaryAttackActive = false, _secondaryAttackActive = true;

        private IReferencesForWeaponContainer _referencesForWeaponContainer = null;

        public void SetReferencesForWeaponContainer(IReferencesForWeaponContainer referencesForWeaponContainer)
        {
            _referencesForWeaponContainer = referencesForWeaponContainer;
        }

        public void ChangeWeapon(AWeapon weaponControllerPrefab)
        {
            if (!Runner.IsServer) return;

            if (_currentWeapon)
                Runner.Despawn(_currentWeapon.Object);

            _currentWeapon = Runner.Spawn(weaponControllerPrefab, _weaponContainer.position, _weaponContainer.rotation, inputAuthority: Runner.LocalPlayer, predictionKey: null);
            _currentWeaponIndexEquipped = _currentWeapon.Id;
        }

        public void StartPrimaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;
            if (_primaryAttackActive) return;


            if (_currentWeapon)
                _currentWeapon.StartPrimaryAttack(aimSource, aimDirection);
            _primaryAttackActive = true;
        }

        public void StartSecondaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;
            if(_secondaryAttackActive) return;

            if (_currentWeapon)
                _currentWeapon.StartSecondaryAttack(aimSource, aimDirection);
            _secondaryAttackActive = true;
        }

        public void StopPrimaryAttack()
        {
            if (!Runner.IsServer) return;
            if (!_primaryAttackActive) return;

            if (_currentWeapon)
                _currentWeapon.StopPrimaryAttack();
            _primaryAttackActive = false;
        }

        public void StopSecondaryAttack()
        {
            if (!Runner.IsServer) return;
            if (!_secondaryAttackActive) return;

            if (_currentWeapon)
                _currentWeapon.StopSecondaryAttack();
            _secondaryAttackActive = false;
        }

        public static void HandleCurrentWeaponIndexEquippedChanged(Changed<HoldedWeaponController> changesHandler)
        {
            if(changesHandler.Behaviour.Runner.TryFindBehaviour(changesHandler.Behaviour._currentWeaponIndexEquipped, out AWeapon weapon))
            {
                changesHandler.Behaviour._currentWeapon = weapon;
                changesHandler.Behaviour._currentWeapon.SetReferencesContainer(changesHandler.Behaviour._referencesForWeaponContainer);
                changesHandler.Behaviour._currentWeapon.GetComponent<AWeaponModelController>().SetModelParent(changesHandler.Behaviour._weaponContainer);
            }
        }

        internal void SetAim(Vector3 aimSource, Vector3 aimDirection)
        {
            (_aimSource, _aimDirection) = (aimSource, aimDirection);
        }

        private void Update()
        {
            if(Runner.IsServer)
                ServerUpdate();
        }

        private void ServerUpdate()
        {
            if(_currentWeapon)
                _currentWeapon.SetAim(_aimSource, _aimDirection);
        }
    }
}