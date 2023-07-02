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
        protected float _healthSliderBGMovementSharpness = 4f;

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

            if(a_healthRatio < m_healthSlider.value)
            {
                HandleDamageTakenFeedback(m_healthSlider.value, a_healthRatio);
            }
            else
            {
                HandleHealedFeedback(m_healthSlider.value, a_healthRatio);
            }

            m_healthSlider.value = a_healthRatio;
        }

        protected virtual void HandleHealedFeedback(float oldHealthRatio, float newHealthRatio)
        {
            m_healthSliderBG.value = newHealthRatio;
        }

        protected virtual void HandleDamageTakenFeedback(float oldHealthRatio, float newHealthRatio)
        {

        }


        protected virtual void Update()
        {
            m_healthSliderBG.value = Mathf.Lerp(m_healthSliderBG.value, m_healthSlider.value, _healthSliderBGMovementSharpness * Time.deltaTime);
        }
    }
}

