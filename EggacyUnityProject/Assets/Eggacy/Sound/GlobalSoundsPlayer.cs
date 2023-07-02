using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Sound
{
    public class GlobalSoundsPlayer : MonoBehaviour
    {
        [SerializeField]
        private GlobalSoundController _soundControllerPrefab = null;

        [SerializeField]
        private List<GlobalSoundEventData> _registeredSoundEventData = new List<GlobalSoundEventData>();

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

        private void HandleSoundPlayRequested(GlobalSoundEventData data)
        {
            PlayGlobalSound(data);
        }

        private void PlayGlobalSound(ASoundEventData soundEventData)
        {
            var soundController = Instantiate(_soundControllerPrefab, this.transform);
            soundController.audioSource.clip = soundEventData.AudioClip;
            soundController.SetLifeDuration(soundEventData.maxLifeTime);

            soundController.audioSource.Play();
        }
    }
}