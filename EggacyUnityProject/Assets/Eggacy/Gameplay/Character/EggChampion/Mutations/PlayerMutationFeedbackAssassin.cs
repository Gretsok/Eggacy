using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class PlayerMutationFeedbackAssassin : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> m_feedbacks;

        [SerializeField]
        private AssassinMutation m_mutation;

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

        private void OnLevelUpdated(AMutation obj)
        {
            foreach (var feedback in m_feedbacks)
            {
                feedback.SetActive(true);
            }
        }
    }
}

