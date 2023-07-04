using Eggacy.Gameplay.Combat.LifeManagement;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class AssailantMutationDataCollector : NetworkBehaviour
    {
        [SerializeField]
        private AssailantMutation _assailantMutation = null;
        [Space]
        [SerializeField]
        private LifeController _lifeController = null;
        [SerializeField]
        private float _experienceMutliplierPerDamage = 1f;

        private void Start()
        {
            _lifeController.onDamageDealt_ServerOnly += HandleDamageDealt;
        }

        private void OnDestroy()
        {
            if(_lifeController)
                _lifeController.onDamageDealt_ServerOnly -= HandleDamageDealt;
        }

        private void HandleDamageDealt(LifeController source, int damage, LifeController victim)
        {
            if(victim.TryGetComponent(out ChickenTank.ChickenTank tank))
            {
                _assailantMutation.EarnExperience((int)(damage * _experienceMutliplierPerDamage));
            }
        }
    }
}