using Eggacy.Gameplay.Combat.TeamManagement;
using Eggacy.Gameplay.Combat.Weapon;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons
{
    public class EggChampionReferencesForWeaponContainer : MonoBehaviour, IReferencesForWeaponContainer
    {
        [SerializeField]
        private TeamController _teamController = null;
        public TeamController teamController => _teamController;
    }
}