using System;
using System.Collections;
using System.Collections.Generic;
using Tween;
using UnityEngine;

namespace Eggacy
{
    public class EggChampionHealthBar : AHealthBar
    {
        [SerializeField]
        private PositionTween m_damageReceivedPositionTween;

        [SerializeField]
        private bool m_useFadeOut = false;

        [SerializeField]
        private float m_timeBeforefadeOut = 0.2f;
        [SerializeField]
        private float m_fadeOutDuration = 0.4f;


        [SerializeField]
        private ColorTween m_healthBarContainerColorTween;
        [SerializeField]
        private ColorTween m_healthSliderColorTween;
        [SerializeField]
        private ColorTween m_healthSliderBGColorTween;

        private void OnDisable()
        {
            m_healthSlider.value = 1f;
            m_healthSliderBG.value = 1f;
        }

        private void Awake()
        {
            m_healthSlider.value = 1f;
            m_healthSliderBG.value = 1f;
          
            m_damageReceivedPositionTween.Stop();
        }

        protected override void HandleDamageTakenFeedback(float oldHealthRatio, float newHealthRatio)
        {
            base.HandleDamageTakenFeedback(oldHealthRatio, newHealthRatio);
            PlayDamageReceivedFeedback();
        }

        public void PlayDamageReceivedFeedback()
        {
            m_damageReceivedPositionTween.StartTween();

            if (m_useFadeOut)
            {
                m_healthBarContainerColorTween.SetTweenDelay(m_timeBeforefadeOut);
                m_healthSliderColorTween.SetTweenDelay(m_timeBeforefadeOut);
                m_healthSliderBGColorTween.SetTweenDelay(m_timeBeforefadeOut);

                m_healthBarContainerColorTween.SetTweenDuration(m_fadeOutDuration);
                m_healthSliderColorTween.SetTweenDuration(m_fadeOutDuration);
                m_healthSliderBGColorTween.SetTweenDuration(m_fadeOutDuration);

                m_healthBarContainerColorTween.Stop();
                m_healthSliderColorTween.Stop();
                m_healthSliderBGColorTween.Stop();

                m_healthBarContainerColorTween.ShowFinalValues();
                m_healthSliderColorTween.ShowFinalValues();
                m_healthSliderBGColorTween.ShowFinalValues();

                m_healthBarContainerColorTween.StartTween();
                m_healthSliderColorTween.StartTween();
                m_healthSliderBGColorTween.StartTween();
            }
        }
    }
}

