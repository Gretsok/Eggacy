using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Weapon
{
    [CreateAssetMenu(fileName = "WeaponsArmoryData", menuName = "Eggacy/Gameplay/Weapon/WeaponsArmoryData")]
    public class WeaponsArmoryData : ScriptableObject
    {
        [SerializeField]
        private List<AWeapon> _weapons = null;
        public int weaponCount => _weapons.Count;
        public AWeapon GetWeaponAt(int index)
        {
            if(index < 0 || index >= _weapons.Count)
            {
                return null;
            }

            return _weapons[index];
        }

        public T GetWeapon<T>() where T : AWeapon
        {
            return _weapons.Find(w => w is T) as T;
        }
    }
}