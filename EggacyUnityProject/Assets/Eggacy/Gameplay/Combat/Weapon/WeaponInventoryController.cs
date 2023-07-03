using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.Weapon
{
    public class WeaponInventoryController : NetworkBehaviour
    {
        [SerializeField]
        private HoldedWeaponController _holdedWeaponController = null;
        [SerializeField]
        private AWeapon _defaultWeapon = null;



        private void Start()
        {
            _holdedWeaponController.ChangeWeapon(_defaultWeapon);
        }

        private void Update()
        {
            if(_isHoldingATemporaryWeapon)
            {
                if(Runner.InterpolationRenderTime - _timeOfStartHolding > _holdingTemporaryWeaponDuration)
                {
                    _isHoldingATemporaryWeapon = false;
                    _holdedWeaponController.ChangeWeapon(_defaultWeapon);
                }
            }
        }

        private bool _isHoldingATemporaryWeapon = false;
        private float _holdingTemporaryWeaponDuration = 1f;
        private float _timeOfStartHolding = 0f;

        public void HoldAWeaponFor(AWeapon weapon, float holdingDuration)
        {
            _timeOfStartHolding = Runner.InterpolationRenderTime;
            _holdingTemporaryWeaponDuration = holdingDuration;
            _holdedWeaponController.ChangeWeapon(weapon);
            _isHoldingATemporaryWeapon = true;
        }

    }
}