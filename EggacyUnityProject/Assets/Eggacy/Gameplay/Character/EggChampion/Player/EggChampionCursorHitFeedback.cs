using Eggacy.Gameplay.Combat.LifeManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player
{
    public class EggChampionCursorHitFeedback : MonoBehaviour
    {
        [SerializeField]
        private Tween.ColorTween m_colorTween;

        [SerializeField]
        private EggChampionPlayerController m_playerController;

        private void Start()
        {
            m_playerController.character.lifeController.onDamageDealt += OnDamageDealt;
        }

        private void OnDestroy()
        {
            m_playerController.character.lifeController.onDamageDealt -= OnDamageDealt;
        }

        private void OnDamageDealt(LifeController lifeController, int damage)
        {
            PlayCursorHitFeedback();
            Debug.Log("CIBLE TOUCHEE");
        }

        public void PlayCursorHitFeedback()
        {
            m_colorTween.Stop();
            m_colorTween.ShowFinalValues();
            m_colorTween.StartTween();
        }
    }
}
