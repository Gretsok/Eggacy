using Eggacy.Gameplay.Combat.LifeManagement;
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
            _firstHealthDisplayContainer.SetName(firstTank.lifeController.teamController.teamData.team.teamName);
            _firstHealthDisplayContainer.SetColor(firstTank.lifeController.teamController.teamData.team.teamColor);
            _firstHealthDisplayContainer.SetHealthRatio((float)firstTank.lifeController.currentLife / (float)firstTank.lifeController.maxLife);
            firstTank.lifeController.onDamageTaken += HandleLifeChangedOnFirstTank;
            firstTank.lifeController.onHealed += HandleLifeChangedOnFirstTank;

            var secondTank = _tankManager.GetTankAt(1);
            _secondHealthDisplayContainer.SetName(secondTank.lifeController.teamController.teamData.team.teamName);
            _secondHealthDisplayContainer.SetColor(secondTank.lifeController.teamController.teamData.team.teamColor);
            _secondHealthDisplayContainer.SetHealthRatio((float)secondTank.lifeController.currentLife / (float)secondTank.lifeController.maxLife);
            secondTank.lifeController.onDamageTaken += HandleLifeChangedOnSecondTank;
            secondTank.lifeController.onHealed += HandleLifeChangedOnSecondTank;
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