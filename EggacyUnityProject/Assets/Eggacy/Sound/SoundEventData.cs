using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Sound
{
    [CreateAssetMenu(fileName = "SoundEventData", menuName = "Eggacy/Sound", order = 1)]
    public class SoundEventData : ScriptableObject
    {
        [SerializeField]
        private AudioClip m_audioClip;
        public AudioClip AudioClip => m_audioClip; 

        public Action<SoundEventData> on2DPlayRequested;
        public Action<SoundEventData, Transform> on3DPlayRequested;


        public void Request2DPlay()
        {
            on2DPlayRequested?.Invoke(this);
        }

        public void Request3DPlay(Transform a_transform)
        {
            on3DPlayRequested?.Invoke(this, a_transform);
        }
    }
}

