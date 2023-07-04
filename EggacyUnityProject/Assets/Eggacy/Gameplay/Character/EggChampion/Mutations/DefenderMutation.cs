using Eggacy.Gameplay.Combat.LifeManagement;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class DefenderMutation : AMutation<DefenderMutationLevelData>
    {
        [SerializeField]
        private LifeController _lifeController = null;

        protected override void HandleReset()
        {
            base.HandleReset();
            _lifeController.SetBonusMaxLife(0);
        }

        protected override void handleLevelIncreased()
        {
            base.handleLevelIncreased();
            _lifeController.SetBonusMaxLife(GetLevelData(level - 1).bonusMaxLife);
        }
    }

    [Serializable]
    public class DefenderMutationLevelData : MutationLevelData
    {
        public int bonusMaxLife = 50;
    }
}