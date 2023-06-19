using System.Collections;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.Result
{
    public class ResultState : LevelFlowState
    {
        [SerializeField]
        private float _displayDuration = 4f;

        protected override IEnumerator HandleServerSetUpRoutine()
        {
            yield return StartCoroutine(base.HandleServerSetUpRoutine());

            yield return new WaitForSeconds(_displayDuration);
            onStateEnded?.Invoke(this);
        }
    }
}