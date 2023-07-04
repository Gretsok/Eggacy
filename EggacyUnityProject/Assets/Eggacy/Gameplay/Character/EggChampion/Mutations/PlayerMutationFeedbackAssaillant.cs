using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class PlayerMutationFeedbackAssaillant : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> m_feedbacks;

        [SerializeField]
        private AssailantMutation m_mutation;

        private void Start()
        {
            m_mutation.onLevelUpdated += OnLevelUpdated;
        }

        private void Awake()
        {
            foreach (var feedback in m_feedbacks)
            {
                feedback.SetActive(false);
            }
        }

        private void OnLevelUpdated(AMutation<AssailantMutationLevelData> obj)
        {
            foreach (var feedback in m_feedbacks)
            {
                feedback.SetActive(true);
            }
        }
    }
}

