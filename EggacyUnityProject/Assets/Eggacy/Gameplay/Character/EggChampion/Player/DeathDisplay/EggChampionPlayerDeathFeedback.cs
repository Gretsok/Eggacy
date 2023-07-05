using Eggacy.Gameplay.Combat.LifeManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player.DeathDisplay
{
    public class EggChampionPlayerDeathFeedback : MonoBehaviour
    {
        [SerializeField]
        private LifeController m_lifeController;

        [SerializeField]
        private GameObject m_deathFX;

        private void Start()
        {
            m_lifeController.onDied += OnDied;
        }

        private void Awake()
        {
            m_deathFX.SetActive(false);
        }

        private void OnDestroy()
        {
            m_lifeController.onDied -= OnDied;
        }

        private void OnDied(LifeController obj)
        {
            m_deathFX.SetActive(true);
            StartCoroutine(WaitBeforeDisable_Routine());
        }

        private IEnumerator WaitBeforeDisable_Routine()
        {
            yield return new WaitForSeconds(2f);
            m_deathFX.SetActive(false);
        }
    }
}