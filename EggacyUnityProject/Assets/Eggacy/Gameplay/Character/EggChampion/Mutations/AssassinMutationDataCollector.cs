using Eggacy.Gameplay.Combat.LifeManagement;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class AssassinMutationDataCollector : NetworkBehaviour
    {
        [SerializeField]
        private AssassinMutation _mutation = null;
        [Space]
        [SerializeField]
        private LifeController _lifeController = null;
        [SerializeField]
        private int _experiencePerKill = 100;

        private void Start()
        {
            _lifeController.onKilled_ServerOnly += HandleKilled;
        }

        private void OnDestroy()
        {
            if (_lifeController)
                _lifeController.onKilled_ServerOnly -= HandleKilled;
        }

        private void HandleKilled(LifeController source, LifeController victim)
        {
            if (victim.TryGetComponent(out ChickenTank.ChickenTank tank))
            {
                _mutation.EarnExperience(_experiencePerKill);
            }
        }

    }
}