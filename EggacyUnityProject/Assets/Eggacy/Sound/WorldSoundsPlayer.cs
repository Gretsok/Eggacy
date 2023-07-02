using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Sound
{
    public class WorldSoundsPlayer : MonoBehaviour
    {
        [SerializeField]
        private WorldSoundController _worldSoundControllerPrefab = null;

        [SerializeField]
        private List<WorldSoundEventData> _registeredSoundEventData = new List<WorldSoundEventData>();

        private void Awake()
        {
            foreach (var soundEventData in _registeredSoundEventData)
            {
                soundEventData.onPlayRequested += HandleSoundPlayRequested;
            }
        }

        private void OnDestroy()
        {
            foreach (var soundEventData in _registeredSoundEventData)
            {
                soundEventData.onPlayRequested -= HandleSoundPlayRequested;
            }
        }

        private void HandleSoundPlayRequested(WorldSoundEventData data, Transform target)
        {
            PlayWorldSound(data, target);
        }

        private void PlayWorldSound(WorldSoundEventData soundEventData, Transform target)
        {
            var soundController = Instantiate(_worldSoundControllerPrefab, target.position, Quaternion.identity, transform);
            soundController.audioSource.clip = soundEventData.AudioClip;
            soundController.SetLifeDuration(soundEventData.maxLifeTime);

            if(soundEventData.attachToTarget)
            {
                soundController.AttachToTarget(target);
            }

            soundController.audioSource.Play();
        }
    }
}