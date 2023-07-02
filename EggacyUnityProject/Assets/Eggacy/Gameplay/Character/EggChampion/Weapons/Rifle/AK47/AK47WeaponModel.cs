using Eggacy.Gameplay.Combat.Weapon;
using Eggacy.Sound;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.Rifle.AK47
{
    public class AK47WeaponModel : AWeaponModel
    {
        [SerializeField]
        protected ParticleSystem _shootFX = null;

        [SerializeField]
        protected WorldSoundEventData _shootSFX = null;
        [SerializeField]
        private Tween.PositionTween a_positionTween;

        internal virtual void PlayShootFeedback()
        {
            if (_shootFX)
                _shootFX.Play();
            if (_shootSFX)
                _shootSFX.RequestWorldSoundPlay(transform);

            a_positionTween.Stop();
            a_positionTween.ShowFinalValues();
            a_positionTween.StartTween();
        }
    }
}