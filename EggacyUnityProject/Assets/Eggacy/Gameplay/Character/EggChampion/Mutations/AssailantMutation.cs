using Eggacy.Gameplay.Character.EggChampion.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class AssailantMutation : AMutation<AssailantMutationLevelData>
    {
        [SerializeField]
        private EggChampionGlobalMutatorsHandler _globalMutatorsHandler = null;

        protected override void HandleReset()
        {
            base.HandleReset();
            _globalMutatorsHandler.SetAttackSpeedMultiplier(1.0f);
            _globalMutatorsHandler.SetDamageMultiplier(1.0f);
        }

        protected override void handleLevelIncreased()
        {
            base.handleLevelIncreased();
            var levelData = GetLevelData(level - 1);
            _globalMutatorsHandler.SetAttackSpeedMultiplier(levelData.attackSpeedMultiplier);
            _globalMutatorsHandler.SetDamageMultiplier(levelData.damageMutliplier);
        }
    }

    [Serializable]
    public class AssailantMutationLevelData : MutationLevelData
    {
        public float damageMutliplier = 0.9f;
        public float attackSpeedMultiplier = 2f;
    }

}