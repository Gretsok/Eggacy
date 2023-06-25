using Eggacy.Sound;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.Weapon
{
    public abstract class AWeaponModel : MonoBehaviour
    {
        [SerializeField]
        protected ParticleSystem _shootFX = null;

        [SerializeField]
        protected SoundEventData _shootSFX = null;

        internal virtual void PlayShootFeedback()
        {
            if (_shootFX)
                _shootFX.Play();
            if (_shootSFX)
                _shootSFX.Request3DPlay(this.transform);
        }
    }
}