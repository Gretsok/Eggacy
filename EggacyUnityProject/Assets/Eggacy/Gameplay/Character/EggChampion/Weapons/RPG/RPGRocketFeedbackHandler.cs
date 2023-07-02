using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.RPG
{
    public class RPGRocketFeedbackHandler : MonoBehaviour
    {
        [SerializeField]
        private RPGRocket _rocket = null;

        [SerializeField]
        private Sound.WorldSoundEventData _travelingSound = null;

        private void Start()
        {
            _travelingSound.RequestWorldSoundPlay(_rocket.explosionSource);
        }
    }
}