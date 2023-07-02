using Eggacy.Gameplay.Character.EggChampion.Weapons.RPG.Feedback;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG
{
    public class RPGRocketFeedbackHandler : MonoBehaviour
    {
        [SerializeField]
        private RPGRocket _rocket = null;

        [SerializeField]
        private Sound.WorldSoundEventData _travelingSound = null;

        [SerializeField]
        private RPGRocketExplosionFeedback _explosionFeedbackPrefab = null;

        private void Start()
        {
            _travelingSound.RequestWorldSoundPlay(_rocket.explosionSource);

            _rocket.onExploded += HandleExplosion;
        }

        private void HandleExplosion()
        {
            Instantiate(_explosionFeedbackPrefab, _rocket.explosionSource.position, Quaternion.identity, null);
        }
    }
}