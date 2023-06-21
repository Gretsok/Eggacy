using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion
{
    public class TankAllyPositionFeedback : MonoBehaviour
    {
        private Transform m_target;

        private ChickenTank.ChickenTank m_allyChickenTank;

        [SerializeField]
        private SpriteRenderer m_spriteRenderer;

        public void Setup(ChickenTank.ChickenTank a_allyChickenTank, Color a_teamColor)
        {
            m_allyChickenTank = a_allyChickenTank;
            m_target = m_allyChickenTank.transform;
            m_spriteRenderer.color = a_teamColor;
        }

        private void LateUpdate()
        {
            if (m_target != null)
            {
                transform.LookAt(m_target);
            }
        }

    }
}

