using Eggacy.Gameplay.Combat.LifeManagement;
using System;
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
            _lifeController.onHealed += OnHealed;
        }

        private void OnHealed(LifeController a_lifeController, int healAMount)
        {
            _healthBar.SetHealthRatio((float)a_lifeController.currentLife / (float)a_lifeController.maxLife);
        }

        private void OnDestroy()
        {
            _lifeController.onDamageTaken -= OnDamageTaken;
            _lifeController.onHealed -= OnHealed;
        }

        private void OnDamageTaken(LifeController a_lifeController, int arg2)
        {
            _healthBar.SetHealthRatio((float)a_lifeController.currentLife / (float)a_lifeController.maxLife);
        }
    }
}

