using Eggacy.Gameplay.Combat.LifeManagement;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.KillBonus
{
    public class KillBonusHandler : MonoBehaviour
    {
        [SerializeField]
        private LifeController _lifeController = null;
        [SerializeField]
        private int _healthToGainPerKill = 30;

        private void Start()
        {
            _lifeController.onKilled_ServerOnly += HandleKilled;
        }

        private void OnDestroy()
        {
            if(_lifeController)
                _lifeController.onKilled_ServerOnly -= HandleKilled;
        }

        private void HandleKilled(LifeController source, LifeController victim)
        {
            source.Heal(_healthToGainPerKill);
        }
    }
}