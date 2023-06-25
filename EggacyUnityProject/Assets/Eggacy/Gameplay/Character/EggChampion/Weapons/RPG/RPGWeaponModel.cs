using Eggacy.Gameplay.Combat.Weapon;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG
{
    public class RPGWeaponModel : AWeaponModel
    {
        [SerializeField]
        private GameObject _rocketModel = null;

        public bool IsRocketModelActive => _rocketModel.activeSelf;

        public void ActivateRocketModel()
        {
            _rocketModel.SetActive(true);
        }

        public void DeactivateRocketModel()
        {
            _rocketModel.SetActive(false);
        }
    }
}