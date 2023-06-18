using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.Weapon
{
    public abstract class AWeaponModel : MonoBehaviour
    {
        [SerializeField]
        protected ParticleSystem _shootFX = null;

        internal virtual void PlayShootFeedback()
        {
            if (_shootFX)
                _shootFX.Play();
        }
    }
}