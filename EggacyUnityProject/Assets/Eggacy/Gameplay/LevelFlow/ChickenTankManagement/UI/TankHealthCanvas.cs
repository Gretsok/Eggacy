using Eggacy.Gameplay.Character.ChickenTank;
using Eggacy.Gameplay.Combat.LifeManagement;
using System.Collections;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.ChickenTankManagement.UI
{
    public class TankHealthCanvas : MonoBehaviour
    {
        [SerializeField]
        private ChickenTankManager _tankManager = null;

        [SerializeField]
        private TankHealthTeamDisplayContainer _firstHealthDisplayContainer = null;
        [SerializeField]
        private TankHealthTeamDisplayContainer _secondHealthDisplayContainer = null;


        private void Awake()
        {
            gameObject.SetActive(false);
            _tankManager.onInitializedForGameplay += HandleManagerIntializedForGameplay;
            _tankManager.onCleanedUpFromGameplay += HandleManagerCleanedUpForGameplay;
        }

        private void HandleManagerCleanedUpForGameplay(ChickenTankManager obj)
        {
            gameObject.SetActive(false);

            var firstTank = _tankManager.GetTankAt(0);
            firstTank.lifeController.onDamageTaken -= HandleLifeChangedOnFirstTank;
            firstTank.lifeController.onHealed -= HandleLifeChangedOnFirstTank;

            var secondTank = _tankManager.GetTankAt(1);
            secondTank.lifeController.onDamageTaken -= HandleLifeChangedOnSecondTank;
            secondTank.lifeController.onHealed -= HandleLifeChangedOnSecondTank;
        }

        private void HandleManagerIntializedForGameplay(ChickenTankManager obj)
        {
            gameObject.SetActive(true);

            var firstTank = _tankManager.GetTankAt(0);
            StartCoroutine(WaitAndSetUpTank(firstTank, _firstHealthDisplayContainer));
            firstTank.lifeController.onDamageTaken += HandleLifeChangedOnFirstTank;
            firstTank.lifeController.onHealed += HandleLifeChangedOnFirstTank;

            var secondTank = _tankManager.GetTankAt(1);
            StartCoroutine(WaitAndSetUpTank(secondTank, _secondHealthDisplayContainer));
            secondTank.lifeController.onDamageTaken += HandleLifeChangedOnSecondTank;
            secondTank.lifeController.onHealed += HandleLifeChangedOnSecondTank;
        }

        private IEnumerator WaitAndSetUpTank(ChickenTank tank, TankHealthTeamDisplayContainer displayContainer)
        {
            while(tank.lifeController.teamController.teamData == null)
            {
                yield return null;
            }
            displayContainer.SetName(tank.lifeController.teamController.teamData.team.teamName);
            displayContainer.SetColor(tank.lifeController.teamController.teamData.team.teamColor);
            displayContainer.SetHealthRatio((float)tank.lifeController.currentLife / (float)tank.lifeController.maxLife);
        }

        private void HandleLifeChangedOnSecondTank(LifeController arg1, int arg2)
        {
            _secondHealthDisplayContainer.SetHealthRatio((float)arg1.currentLife / (float)arg1.maxLife);
        }

        private void HandleLifeChangedOnFirstTank(LifeController arg1, int arg2)
        {
            _firstHealthDisplayContainer.SetHealthRatio((float)arg1.currentLife / (float)arg1.maxLife);
        }
    }
}