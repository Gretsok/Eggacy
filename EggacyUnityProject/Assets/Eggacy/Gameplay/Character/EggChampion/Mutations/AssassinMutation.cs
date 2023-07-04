using System;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class AssassinMutation : AMutation<AssassinMutationLevelData>
    {
        [SerializeField]
        private EggChampionCharacter _character = null;

        protected override void HandleReset()
        {
            base.HandleReset();
            _character.SetSpeedMultiplier(1.0f);
        }

        protected override void handleLevelIncreased()
        {
            base.handleLevelIncreased();
            _character.SetSpeedMultiplier(GetLevelData(level - 1).movementSpeedMultiplier);
        }
    }


    [Serializable]
    public class AssassinMutationLevelData : MutationLevelData
    {
        public float movementSpeedMultiplier = 1.3f;
    }
}