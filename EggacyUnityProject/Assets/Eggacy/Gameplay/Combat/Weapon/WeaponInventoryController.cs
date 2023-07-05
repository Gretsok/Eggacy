using Fusion;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.Weapon
{
    public class WeaponInventoryController : NetworkBehaviour
    {
        [Serializable]
        public enum ECurrentWeaponBehaviour
        {
            NormallyEquipped,
            Temporary,
            LimitedNumberOfAttack
        }
        [SerializeField]
        private HoldedWeaponController _holdedWeaponController = null;
        [SerializeField]
        private AWeapon _defaultWeapon = null;


        private ECurrentWeaponBehaviour _currentWeaponBehaviour = ECurrentWeaponBehaviour.NormallyEquipped;

        private void Start()
        {
            _holdedWeaponController.ChangeWeapon(_defaultWeapon);
        }

        private void Update()
        {
            if(_currentWeaponBehaviour == ECurrentWeaponBehaviour.Temporary)
            {
                UpdateTemporaryWeaponBehaviour();
            }
        }

        #region Temporary Weapon
        private float _holdingTemporaryWeaponDuration = 1f;
        private float _timeOfStartHolding = 0f;

        public void HoldATemporaryWeapon(AWeapon weapon, float holdingDuration)
        {
            _timeOfStartHolding = Runner.InterpolationRenderTime;
            _holdingTemporaryWeaponDuration = holdingDuration;
            _holdedWeaponController.ChangeWeapon(weapon);
            _currentWeaponBehaviour = ECurrentWeaponBehaviour.Temporary;
        }

        private void UpdateTemporaryWeaponBehaviour()
        {
            if (Runner.InterpolationRenderTime - _timeOfStartHolding > _holdingTemporaryWeaponDuration)
            {
                _currentWeaponBehaviour = ECurrentWeaponBehaviour.NormallyEquipped;
                _holdedWeaponController.ChangeWeapon(_defaultWeapon);
            }
        }
        #endregion

        #region Limited Number of Attacks
        private int _numberOfAttacksLeft = 0;
        public void HoldAWeaponForALimitedNumberOfAttack(AWeapon weapon, int numberOfAttacks)
        {
            _numberOfAttacksLeft = numberOfAttacks;
            _holdedWeaponController.ChangeWeapon(weapon);
            _holdedWeaponController.currentWeapon.onAttack_ServerOnly += HanlePrimaryAttackOnWeaponLimitedOnNumberOfAttacks;
            _currentWeaponBehaviour = ECurrentWeaponBehaviour.LimitedNumberOfAttack;
        }

        private void HanlePrimaryAttackOnWeaponLimitedOnNumberOfAttacks()
        {
            --_numberOfAttacksLeft;
            if(_numberOfAttacksLeft <= 0)
            {
                _holdedWeaponController.currentWeapon.onAttack_ServerOnly -= HanlePrimaryAttackOnWeaponLimitedOnNumberOfAttacks;
                _holdedWeaponController.ChangeWeapon(_defaultWeapon);
                _currentWeaponBehaviour = ECurrentWeaponBehaviour.NormallyEquipped;
            }
        }
        #endregion
    }
}