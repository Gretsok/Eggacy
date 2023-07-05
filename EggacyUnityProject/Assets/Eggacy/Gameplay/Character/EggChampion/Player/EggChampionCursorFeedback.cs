using Eggacy.Gameplay.Combat.LifeManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player
{
    public class EggChampionCursorFeedback : MonoBehaviour
    {
        [SerializeField]
        private Tween.ColorTween m_colorTweenHit;

        [SerializeField]
        private Tween.ColorTween m_colorTweenKill;

        [SerializeField]
        private EggChampionPlayerController m_playerController;

        private void Start()
        {
            m_playerController.character.lifeController.onDamageDealt += OnDamageDealt;
            m_playerController.character.lifeController.onKilled += OnKilled;
        }

        private void OnDestroy()
        {
            m_playerController.character.lifeController.onDamageDealt -= OnDamageDealt;
            m_playerController.character.lifeController.onKilled -= OnKilled;
        }

        private void OnKilled(LifeController obj)
        {
            PlayCursorKillFeedback();
        }

        private void PlayCursorKillFeedback()
        {
            m_colorTweenKill.Stop();
            m_colorTweenKill.ShowFinalValues();
            m_colorTweenKill.StartTween();
        }

        private void OnDamageDealt(LifeController lifeController, int damage)
        {
            PlayCursorHitFeedback();
        }

        public void PlayCursorHitFeedback()
        {
            m_colorTweenHit.Stop();
            m_colorTweenHit.ShowFinalValues();
            m_colorTweenHit.StartTween();
        }
    }
}
