using System;
using UnityEngine;

namespace Eggacy.Sound
{
    [CreateAssetMenu(fileName = "WorldSoundEventData", menuName = "Eggacy/Sound/WorldSoundEventData")]

    public class WorldSoundEventData : ASoundEventData
    {
        [SerializeField]
        private bool _attachToTarget = false;
        public bool attachToTarget => _attachToTarget;

        [SerializeField]
        private float _maxDistanceToHear = 300f;
        public float maxDistanceToHear => maxDistanceToHear;

        public Action<WorldSoundEventData, Transform> onPlayRequested;

        public void RequestWorldSoundPlay(Transform target)
        {
            onPlayRequested?.Invoke(this, target);
        }
    }
}