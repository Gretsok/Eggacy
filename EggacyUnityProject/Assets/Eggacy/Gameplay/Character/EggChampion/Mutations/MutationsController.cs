using Eggacy.Gameplay.Combat.LifeManagement;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class MutationsController : NetworkBehaviour
    {
        [SerializeField]
        private DefenderMutation _defenderMutation = null;
        public DefenderMutation defenderMutation => _defenderMutation;

        [SerializeField]
        private AssailantMutation _assailantMutation = null;
        public AssailantMutation assailantMutation => _assailantMutation;

        [SerializeField]
        private AssassinMutation _assassinMutation = null;
        public AssassinMutation assassinMutation => _assassinMutation;

        [Space]
        [SerializeField]
        private LifeController _lifeController = null;
        public LifeController lifeController => _lifeController;

        private void Start()
        {
            if(_lifeController)
            {
                _lifeController.onDied_ServerOnly += HandledDied;
            }
        }

        private void HandledDied(LifeController obj)
        {
            _defenderMutation.ResetLevelsAndExperience();
            _assailantMutation.ResetLevelsAndExperience();
            _assassinMutation.ResetLevelsAndExperience();
        }

        private void OnDestroy()
        {
            if (_lifeController)
            {
                _lifeController.onDied_ServerOnly -= HandledDied;
            }
        }
    }
}