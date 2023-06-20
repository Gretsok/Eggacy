using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Eggacy
{
    public abstract class AHealthBar : MonoBehaviour
    {
        [SerializeField]
        protected Image m_healthBarContainer;
        [SerializeField]
        protected Slider m_healthSlider;
        [SerializeField]
        protected Slider m_healthSliderBG;

        [SerializeField]
        protected float m_updateSliderBGSpeed = 3f;
        [SerializeField]
        protected float m_timeBeforeUpdateSlider = 0.2f;

        protected Color m_healBarContainerColor;
        protected Color m_healthSliderColor;
        protected Color m_healthSliderBGColor;

        protected virtual void Start()
        {
            m_healBarContainerColor = m_healthBarContainer.color;
            m_healthSliderColor = m_healthSlider.colors.normalColor; 
            m_healthSliderBGColor = m_healthSliderBG.colors.normalColor;

            m_healthSlider.value = 1f;
            m_healthSliderBG.value = 1f;
        }

        public virtual void SetHealthRatio(float a_healthRatio)
        {
            a_healthRatio = Mathf.Clamp(a_healthRatio, 0f, 1f);

            m_healthSlider.value = a_healthRatio;

            StopAllCoroutines();
            StartCoroutine(UpdateHealthSliderBG_Routine());
        }


        private IEnumerator UpdateHealthSliderBG_Routine()
        {
            yield return new WaitForSeconds(m_timeBeforeUpdateSlider);

            while(m_healthSliderBG.value > m_healthSlider.value)
            {
                m_healthSliderBG.value -= 0.01f * m_updateSliderBGSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}

