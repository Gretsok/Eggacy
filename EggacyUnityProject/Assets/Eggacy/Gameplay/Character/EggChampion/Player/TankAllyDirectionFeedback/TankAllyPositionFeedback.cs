using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion
{
    public class TankAllyPositionFeedback : MonoBehaviour
    {
        private Transform m_target;

        private ChickenTank.ChickenTank m_allyChickenTank;

        public void SetAllyChickenTank(ChickenTank.ChickenTank a_allyChickenTank)
        {
            m_allyChickenTank = a_allyChickenTank;
            m_target = m_allyChickenTank.transform;
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

