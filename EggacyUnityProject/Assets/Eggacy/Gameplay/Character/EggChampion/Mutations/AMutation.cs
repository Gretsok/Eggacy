using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class AMutation : NetworkBehaviour
    {
        #region Level Management
        [Networked(OnChanged = nameof(HandleLevelChanged))]
        private int _level { get; set; }

        public Action<AMutation> onLevelUpdated = null;

        public int level => _level;

        [Networked(OnChanged = nameof(HandleExperienceChanged))]
        private int _currentExperience { get; set; }
        public int currentExperience => _currentExperience;

        public Action<AMutation> onExperienceUpdated = null;

        public void EarnExperience(int amountOfExperience)
        {
            if (!Runner.IsServer) return;

            var newExperience = amountOfExperience + _currentExperience;

            _currentExperience = ProcessExperience(newExperience);
            HandleExperienceEarned();
        }

        protected virtual int ProcessExperience(int newExperience)
        {
            return newExperience;
        }

        public void ResetLevelsAndExperience()
        {
            if (!Runner.IsServer) return;

            _level = 0;
            _currentExperience = 0;
            HandleReset();
        }

        public void IncreaseLevel()
        {
            _level += 1;
            handleLevelIncreased();
        }

        #region Network Changes Callbacks
        public static void HandleLevelChanged(Changed<AMutation> changesHandler)
        {
            changesHandler.Behaviour.HandleLevelChangedNetworkCallback();
        }

        protected virtual void HandleLevelChangedNetworkCallback()
        {

        }

        public static void HandleExperienceChanged(Changed<AMutation> changesHandler)
        {
            changesHandler.Behaviour.HandleExperienceChangedNetworkCallback();
        }

        protected virtual void HandleExperienceChangedNetworkCallback()
        {

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
    }
    public class AMutation<T> : AMutation where T : MutationLevelData
    {
        [SerializeField]
        private List<T> _levelData = new List<T>();

        public override void Spawned()
        {
            base.Spawned();
            ResetLevelsAndExperience();
        }

        public T GetLevelData(int level)
        {
            return _levelData[level];
        }

        public int levelDataCount => _levelData.Count;

        protected override int ProcessExperience(int newExperience)
        {
            if (level >= _levelData.Count) return newExperience; // Level Max reached, so we cannot earn more experience


            while (level < _levelData.Count && newExperience >= _levelData[level].xpRequiredToLevelUp)
            {
                newExperience -= _levelData[level].xpRequiredToLevelUp;
                IncreaseLevel();
            }

            return newExperience;
        }

        protected override void HandleLevelChangedNetworkCallback()
        {
            base.HandleLevelChangedNetworkCallback();
            onLevelUpdated?.Invoke(this);
        }

        protected override void HandleExperienceChangedNetworkCallback()
        {
            base.HandleExperienceChangedNetworkCallback();
            onExperienceUpdated?.Invoke(this);
        }
    }

    [Serializable]
    public class MutationLevelData
    {
        public int xpRequiredToLevelUp = 100;
    }

}