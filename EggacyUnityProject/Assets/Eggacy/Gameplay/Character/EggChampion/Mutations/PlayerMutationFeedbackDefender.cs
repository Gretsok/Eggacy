using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class PlayerMutationFeedbackDefender : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> m_feedbacks;

        [SerializeField]
        private DefenderMutation m_mutation;

        private void Start()
        {
            m_mutation.onLevelUpdated += OnLevelUpdated;
        }

        private void OnDestroy()
        {
            m_mutation.onLevelUpdated -= OnLevelUpdated;
        }

        private void Awake()
        {
            foreach (var feedback in m_feedbacks)
            {
                feedback.SetActive(false);
            }
        }

        private void OnLevelUpdated(AMutation<DefenderMutationLevelData> mutation)
        {
            foreach (var feedback in m_feedbacks)
            {
                feedback.SetActive(mutation.level >= 1);
            }
        }
    }
}

