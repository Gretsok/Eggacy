using Eggacy.Gameplay.Character.EggChampion.Player;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class PlayerMutationsCanvas : MonoBehaviour
    {
        [SerializeField]
        private EggChampionPlayerController _playerController = null;

        [SerializeField]
        private PlayerMutationWidget m_mutationWidgetAssaillant;

        [SerializeField]
        private PlayerMutationWidget m_mutationWidgetAssassin;

        [SerializeField]
        private PlayerMutationWidget m_mutationWidgetDefender;

        private void Start()
        {
            _playerController.character.mutationsController.defenderMutation.onExperienceUpdated += HandleDefenderMutationExperienceUpdated;
            _playerController.character.mutationsController.assailantMutation.onExperienceUpdated += HandleAssailantMutationExperienceUpdated;
            _playerController.character.mutationsController.assassinMutation.onExperienceUpdated += HandleAssassinMutationExperienceUpdated;

            _playerController.character.mutationsController.defenderMutation.onLevelUpdated += HandleDefenderMutationLevelUpdated;
            _playerController.character.mutationsController.assailantMutation.onLevelUpdated += HandleAssailantMutationLevelUpdated;
            _playerController.character.mutationsController.assassinMutation.onLevelUpdated += HandleAssassinMutationLevelUpdated;

            HandleDefenderMutationExperienceUpdated(_playerController.character.mutationsController.defenderMutation);
            HandleAssailantMutationExperienceUpdated(_playerController.character.mutationsController.assailantMutation);
            HandleAssassinMutationExperienceUpdated(_playerController.character.mutationsController.assassinMutation);

            HandleDefenderMutationLevelUpdated(_playerController.character.mutationsController.defenderMutation);
            HandleAssailantMutationLevelUpdated(_playerController.character.mutationsController.assailantMutation);
            HandleAssassinMutationLevelUpdated(_playerController.character.mutationsController.assassinMutation);
        }

        private void HandleAssassinMutationLevelUpdated(AMutation<AssassinMutationLevelData> mutation)
        {
            m_mutationWidgetAssassin.SetMutationAvailable(mutation.level != 0);
        }

        private void HandleDefenderMutationLevelUpdated(AMutation<DefenderMutationLevelData> mutation)
        {
            m_mutationWidgetDefender.SetMutationAvailable(mutation.level != 0);
        }

        private void HandleAssailantMutationLevelUpdated(AMutation<AssailantMutationLevelData> mutation)
        {
            m_mutationWidgetAssaillant.SetMutationAvailable(mutation.level != 0);
        }

        private void OnDestroy()
        {
            _playerController.character.mutationsController.defenderMutation.onExperienceUpdated -= HandleDefenderMutationExperienceUpdated;
            _playerController.character.mutationsController.assailantMutation.onExperienceUpdated -= HandleAssailantMutationExperienceUpdated;
            _playerController.character.mutationsController.assassinMutation.onExperienceUpdated -= HandleAssassinMutationExperienceUpdated;

            _playerController.character.mutationsController.defenderMutation.onLevelUpdated -= HandleDefenderMutationLevelUpdated;
            _playerController.character.mutationsController.assailantMutation.onLevelUpdated -= HandleAssailantMutationLevelUpdated;
            _playerController.character.mutationsController.assassinMutation.onLevelUpdated -= HandleAssassinMutationLevelUpdated;
        }

        private void HandleAssassinMutationExperienceUpdated(AMutation<AssassinMutationLevelData> mutation)
        {
            if (mutation.level < mutation.levelDataCount)
                m_mutationWidgetAssassin.SetMutationCompletion((float)mutation.currentExperience / (float)mutation.GetLevelData(mutation.level).xpRequiredToLevelUp);
            else
                m_mutationWidgetAssassin.SetMutationCompletion(1f);
        }

        private void HandleAssailantMutationExperienceUpdated(AMutation<AssailantMutationLevelData> mutation)
        {
            if (mutation.level < mutation.levelDataCount)
                m_mutationWidgetAssaillant.SetMutationCompletion((float)mutation.currentExperience / (float)mutation.GetLevelData(mutation.level).xpRequiredToLevelUp);
            else
                m_mutationWidgetAssaillant.SetMutationCompletion(1f);
        }

        private void HandleDefenderMutationExperienceUpdated(AMutation<DefenderMutationLevelData> mutation)
        {
            if (mutation.level < mutation.levelDataCount)
                m_mutationWidgetDefender.SetMutationCompletion((float)mutation.currentExperience / (float)mutation.GetLevelData(mutation.level).xpRequiredToLevelUp);
            else
                m_mutationWidgetDefender.SetMutationCompletion(1f);
        }
    }
}