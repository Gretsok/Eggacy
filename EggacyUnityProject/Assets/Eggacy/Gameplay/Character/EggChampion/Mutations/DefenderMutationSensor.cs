using Eggacy.Gameplay.Character.ChickenTank;
using Eggacy.Gameplay.Combat.TeamManagement;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations
{
    public class DefenderMutationSensor : NetworkBehaviour
    {
        [SerializeField]
        private DefenderMutation _mutation = null;
        [Space]
        [SerializeField]
        private TeamController _teamController = null;
        [SerializeField]
        private Transform _detectionSource = null;
        [SerializeField]
        private float _detectionRadius = 5f;
        [SerializeField]
        private float _sensingCooldown = 0.5f;
        private float _lastTimeOfSense = float.MinValue;
        [SerializeField]
        private int _experienceToEarnWhenCloseToAlliedChickenTank = 5;
#if UNITY_EDITOR
        [Space]
        [Header("Editor Only")]
        [SerializeField]
        private bool _drawGizmos = true;
#endif
        private void Update()
        {
            if(Time.time - _lastTimeOfSense > _sensingCooldown)
            {
                Sense();
                _lastTimeOfSense = Time.time;
            }
        }

        private void Sense()
        {
            if (!Runner.IsServer) return;

            var colliders = Physics.OverlapSphere(_detectionSource.position, _detectionRadius);
            bool alliedChickenTankInContact = false;
            foreach(Collider collider in colliders)
            {
                if(collider && collider.TryGetComponent(out ChickenTankCollider chickenTankCollider)
                    && chickenTankCollider.chickenTank.teamController.teamData == _teamController.teamData)
                {
                    alliedChickenTankInContact = true;
                }
            }

            if(alliedChickenTankInContact)
            {
                _mutation.EarnExperience(_experienceToEarnWhenCloseToAlliedChickenTank);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_drawGizmos) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_detectionSource.position, _detectionRadius);
        }
#endif
    }
}