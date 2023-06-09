using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class PlayerMutationWidget : MonoBehaviour
    {
        [SerializeField]
        private Image m_filler;

        [SerializeField]
        private Image m_mutationIcon;

        [SerializeField]
        private Color m_availableColor;
        [SerializeField]
        private Color m_disabledColor;

        [SerializeField]
        private Tween.ScaleTween m_scaleTween;

        public void SetMutationCompletion(float a_fillRatio)
        {
            m_filler.fillAmount = a_fillRatio;
        }

        public void SetMutationAvailable(bool a_isAvailable)
        {
            m_mutationIcon.color = a_isAvailable ? m_availableColor : m_disabledColor;

            if(m_scaleTween != null) 
            {
                if (a_isAvailable)
                {
                    m_scaleTween.StartTween();
                }
                else
                {
                    m_scaleTween.Stop();
                    m_mutationIcon.transform.localScale = Vector3.one;
                }
            }
        }
    }
}