using UnityEngine;

namespace Eggacy.Sound
{
    public class WorldSoundController : ASoundController
    {
        [SerializeField]
        private AudioSource _audioSource = null;
        public AudioSource audioSource => _audioSource;

        private Transform _target = null;
        private bool _attachToTarget = false;

        public void AttachToTarget(Transform target)
        {
            _target = target;
            if(_target)
            {
                _attachToTarget = true;
            }
        }

        public void SetMaxDistanceToHearSound(float maxDistanceToHear)
        {
            _audioSource.maxDistance = maxDistanceToHear;
        }

        protected override void Update()
        {
            base.Update();

            if(_attachToTarget)
            {
                if(_target)
                {
                    transform.position = _target.position;
                }
                else
                {
                    StopSound();
                }
            }
        }

    }
}