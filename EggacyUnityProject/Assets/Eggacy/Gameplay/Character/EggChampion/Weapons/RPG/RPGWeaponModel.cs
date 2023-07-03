using Eggacy.Gameplay.Combat.Weapon;
using Eggacy.Sound;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG
{
    public class RPGWeaponModel : AWeaponModel
    {
        [SerializeField]
        private GameObject _rocketModel = null;

        [SerializeField]
        private GameObject _flashModel = null;

        [SerializeField]
        protected WorldSoundEventData _shootSFX = null;

        [SerializeField]
        private Transform _shootSource = null;

        public bool IsRocketModelActive => _rocketModel.activeSelf;

        public void ActivateRocketModel()
        {
            _rocketModel.SetActive(true);
            _flashModel.SetActive(false);
        }

        public void DeactivateRocketModel()
        {
            _rocketModel.SetActive(false);
            _flashModel.SetActive(true);
        }

        public void PlayShootFeedback()
        {
            if (_shootSFX)
                _shootSFX.RequestWorldSoundPlay(_shootSource);
        }
    }
}