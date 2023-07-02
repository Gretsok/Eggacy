using UnityEngine;

namespace Eggacy.Sound
{
    public abstract class ASoundController : MonoBehaviour
    {
        private float _lifeDuration = 10f;
        private float _timeOfStart = float.MinValue;

        protected virtual void Start()
        {
            _timeOfStart = Time.time;
        }

        protected virtual void Update()
        {
            if(Time.time - _timeOfStart > _lifeDuration)
            {
                StopSound();
            }
        }

        public void SetLifeDuration(float lifeDuration)
        {
            _lifeDuration = lifeDuration;
        }

        public void StopSound()
        {
            Destroy(gameObject);
        }
    }
}