using UnityEngine;
using UnityEngine.UI;

namespace Tween
{
    [System.Serializable]
    public class ColorTween : ATween
    {
        [SerializeField]
        private Color m_initialValue;
        [SerializeField]
        private Color m_finalValue;

        private Image m_imageTarget = null;

        protected override void Start()
        {
            base.Start();
            m_imageTarget = m_target.GetComponent<Image>();
        }

        protected override void SetFinalValues()
        {
            base.SetFinalValues();
            m_imageTarget.color = m_initialValue;
        }

        protected override void SetStartingValues()
        {
            base.SetStartingValues();
            m_imageTarget.color = m_finalValue;
        }

        protected override void ManageTween(float interpolationValue)
        {
            base.ManageTween(interpolationValue);
            m_imageTarget.color = Color.Lerp(m_initialValue, m_finalValue, interpolationValue);
        }
    }
}
