using Eggacy.Sound;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG.Feedback
{
    public class RPGRocketExplosionFeedback : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _explosionVFX = null;
        [SerializeField]
        private WorldSoundEventData _explosionSFX = null;

        [SerializeField]
        private float _lifeSpan = 3f;
        private float _lifeOfStart = float.MaxValue;

        private void Start()
        {
            _explosionVFX.Play();
            _explosionSFX.RequestWorldSoundPlay(transform);

            _lifeOfStart = Time.time;
        }

        private void Update()
        {
            if(Time.time - _lifeOfStart > _lifeSpan)
            {
                Destroy(gameObject);
            }
        }
    }
}