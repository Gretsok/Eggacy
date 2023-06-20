using Eggacy.Gameplay.Combat.LifeManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank
{
    public class ChickenTankHealthDisplayManager : MonoBehaviour
    {
        [SerializeField]
        private LifeController _lifeController;
        [SerializeField]
        private EggChampionHealthBar _healthBar;

        private void Start()
        {
            _healthBar.SetHealthRatio(1f);

            _lifeController.onDamageTaken += OnDamageTaken;
            Debug.Log("Setup health ratio");
        }

        private void OnDestroy()
        {
            _lifeController.onDamageTaken -= OnDamageTaken;
        }

        private void OnDamageTaken(LifeController a_lifeController, int arg2)
        {
            _healthBar.SetHealthRatio((float)a_lifeController.currentLife / (float)a_lifeController.maxLife);
            Debug.Log("Current life : " + a_lifeController.currentLife);
            Debug.Log("Max life : " + a_lifeController.maxLife);
        }
    }
}

