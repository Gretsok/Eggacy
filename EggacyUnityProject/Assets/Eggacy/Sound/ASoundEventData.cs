using System;
using UnityEngine;

namespace Eggacy.Sound
{
    public abstract class ASoundEventData : ScriptableObject
    {
        [SerializeField]
        private AudioClip m_audioClip;
        public AudioClip AudioClip => m_audioClip;

        [SerializeField]
        private float _maxLifeTime = 1000;
        public float maxLifeTime => _maxLifeTime;
    }
}

