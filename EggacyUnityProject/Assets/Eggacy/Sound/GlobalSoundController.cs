
using UnityEngine;

namespace Eggacy.Sound
{
    public class GlobalSoundController : ASoundController
    {
        [SerializeField]
        private AudioSource _audioSource = null;
        public AudioSource audioSource => _audioSource;
    }
}