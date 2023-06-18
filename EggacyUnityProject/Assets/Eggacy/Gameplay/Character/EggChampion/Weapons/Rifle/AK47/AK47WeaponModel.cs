using Eggacy.Gameplay.Combat.Weapon;
using System;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Weapons.Rifle.AK47
{
    public class AK47WeaponModel : AWeaponModel
    {
        [SerializeField]
        private Tween.PositionTween a_positionTween;

        internal override void PlayShootFeedback()
        {
            base.PlayShootFeedback();
            a_positionTween.Stop();
            a_positionTween.ShowFinalValues();
            a_positionTween.StartTween();
        }
    }
}