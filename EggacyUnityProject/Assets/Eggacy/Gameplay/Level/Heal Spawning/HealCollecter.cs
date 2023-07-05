using Eggacy.Gameplay.Combat.LifeManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Level
{
    public class HealCollecter : MonoBehaviour
    {
        [SerializeField]
        private LifeController m_lifeController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HealSpawner spawner))
            {
                var heal = spawner.TryToGetHealPickedUp();

                if (heal)
                {
                    if(m_lifeController.currentLife < m_lifeController.maxLife)
                    {
                        m_lifeController.Heal((int)spawner.healAmount);
                    }
                }
            }
        }
    }
}

