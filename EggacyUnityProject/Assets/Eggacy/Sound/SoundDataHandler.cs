using UnityEngine;

namespace Eggacy.Sound
{
    public class SoundDataHandler : MonoBehaviour
    {
        private float _lifeDuration = 10f;
        private float _timeOfStart = float.MinValue;

        private void Start()
        {
            _timeOfStart = Time.time;
        }

        private void Update()
        {
            if(Time.time - _timeOfStart > _lifeDuration)
            {
                Destroy(gameObject);
            }
        }

        public void SetLifeDuration(float lifeDuration)
        {
            _lifeDuration = lifeDuration;
        }
    }
}