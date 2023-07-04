using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons
{
    public class EggChampionGlobalMutatorsHandler : MonoBehaviour
    {
        public float attackSpeedMultiplier { get; private set; }
        public float damageMultiplier { get; private set; }

        public void SetAttackSpeedMultiplier(float shootingSpeedMultiplier)
        {
            this.attackSpeedMultiplier = shootingSpeedMultiplier;
        }

        public void SetDamageMultiplier(float damageMultiplier)
        {
            this.damageMultiplier = damageMultiplier;
        }
    }
}