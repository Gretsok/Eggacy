using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class AMutation : NetworkBehaviour
    {

    }
    public class AMutation<T> : AMutation where T : MutationLevelData
    {
        public override void Spawned()
        {
            base.Spawned();
            ResetLevelsAndExperience();
        }

        #region Level Management

        [SerializeField]
        private List<T> _levelData = new List<T>();

        [Networked(OnChanged = nameof(HandleLevelChanged))]
        private int _level { get; set; }

        public Action<AMutation<T>> onLevelUpdated = null;

        public int level => _level;

        [Networked(OnChanged = nameof(HandleExperienceChanged))]
        private int _currentExperience { get; set; }
        public int currentExperience => _currentExperience;

        public Action<AMutation<T>> onExperienceUpdated = null;

        public void EarnExperience(int amountOfExperience)
        {
            if (!Runner.IsServer) return;
            if (level >= _levelData.Count) return; // Level Max reached, so we cannot earn more experience

            var newExperience = amountOfExperience + _currentExperience;
            while(level < _levelData.Count && newExperience > _levelData[_level].xpRequiredToLevelUp)
            {
                newExperience -= _levelData[_level].xpRequiredToLevelUp;
                IncreaseLevel();
            }


            _currentExperience = newExperience;
            HandleExperienceEarned(); 
        }

        public void ResetLevelsAndExperience()
        {
            if (!Runner.IsServer) return;

            _level = 0;
            _currentExperience = 0;
            HandleReset();
        }

        private void IncreaseLevel()
        {
            _level += 1;
            handleLevelIncreased();
        }

        #region Network Changes Callbacks
        public static void HandleLevelChanged(Changed<AMutation> changesHandler)
        {
            (changesHandler.Behaviour as AMutation<T>).onLevelUpdated?.Invoke((changesHandler.Behaviour as AMutation<T>));
        }

        public static void HandleExperienceChanged(Changed<AMutation> changesHandler)
        {
            (changesHandler.Behaviour as AMutation<T>).onExperienceUpdated?.Invoke((changesHandler.Behaviour as AMutation<T>));
        }
        #endregion
        #endregion

        #region Feedback Handling
        protected virtual void HandleReset()
        {

        }

        protected virtual void HandleExperienceEarned()
        {

        }

        protected virtual void handleLevelIncreased()
        {

        }
        #endregion

        public T GetLevelData(int level)
        {
            return _levelData[level];
        }

        public int levelDataCount => _levelData.Count;
    }

    [Serializable]
    public class MutationLevelData
    {
        public int xpRequiredToLevelUp = 100;
    }

}