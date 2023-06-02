using System.Collections;
using System.Collections.Generic;
using Tween;
using UnityEngine;

namespace Eggacy
{
    public class EggChampionHealthBar : AHealthBar
    {
        [SerializeField]
        private PositionTween a_damageReceivedPositionTween;

        private void Start()
        {
            m_healthSlider.value = 1f;
            m_healthSliderBG.value = 1f;
        }

        public override void SetHealthRatio(float a_healthRatio)
        {
            base.SetHealthRatio(a_healthRatio);
            PlayDamageReceivedFeedback();
        }

        public void PlayDamageReceivedFeedback()
        {
            a_damageReceivedPositionTween.StartTween();
        }
    }
}

