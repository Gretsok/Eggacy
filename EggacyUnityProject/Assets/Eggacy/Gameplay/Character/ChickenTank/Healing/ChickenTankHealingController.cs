using Eggacy.Gameplay.Combat.LifeManagement;
using Eggacy.Gameplay.Combat.TeamManagement;
using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank.Healing
{
    public class ChickenTankHealingController : MonoBehaviour
    {
        [SerializeField]
        private TeamController _teamController = null;
        [SerializeField]
        private Transform _healingZoneCenter = null;
        [SerializeField]
        private float _zoneRadius = 10f;
        [SerializeField]
        private float _healCooldown = 0.5f;
        [SerializeField]
        private int _amountToHeal = 5;
        private float _lastTimeHealed = float.MinValue;

        private void Update()
        {
            if(Time.time - _lastTimeHealed > _healCooldown)
            {
                Heal();
                _lastTimeHealed = Time.time;
            }
        }

        private void Heal()
        {
            var colliders = Physics.OverlapSphere(_healingZoneCenter.position, _zoneRadius);

            for(int i = 0; i <  colliders.Length; ++i)
            {
                if (colliders[i].TryGetComponent<LifeControllerCollider>(out LifeControllerCollider lifeControllerCollider) 
                    && lifeControllerCollider.lifeController.teamController.teamData.instanceIndex == _teamController.teamData.instanceIndex)
                {
                    lifeControllerCollider.lifeController.Heal(_amountToHeal);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_healingZoneCenter.position, _zoneRadius);
        }
#endif
    }
}