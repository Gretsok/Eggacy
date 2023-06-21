using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Sound
{
    public class SoundPoolManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource m_audioSource;

        public List<SoundEventData> RegisteredSoundEventData;

        private void Awake()
        {
            foreach (var soundEventData in RegisteredSoundEventData)
            {
                soundEventData.on2DPlayRequested += Play2DSound;
                soundEventData.on3DPlayRequested += Play3DSound;
            }
        }

        private void Play3DSound(SoundEventData soundEventData, Transform transform)
        {
            var audioSource = Instantiate(m_audioSource, transform.position, Quaternion.identity, this.transform);
            audioSource.clip = soundEventData.AudioClip;
            audioSource.spatialBlend = 1f;
            if(audioSource.TryGetComponent<SoundDataHandler>(out SoundDataHandler handler))
            {
                handler.SetLifeDuration(soundEventData.maxLifeTime);
            }
            audioSource.Play();
        }

        private void Play2DSound(SoundEventData soundEventData)
        {
            var audioSource = Instantiate(m_audioSource, this.transform);
            audioSource.clip = soundEventData.AudioClip;
            audioSource.spatialBlend = 0f;
            if (audioSource.TryGetComponent<SoundDataHandler>(out SoundDataHandler handler))
            {
                handler.SetLifeDuration(soundEventData.maxLifeTime);
            }
            audioSource.Play();
        }
    }
}

