using Eggacy.Gameplay.Character.ChickenTank;
using System.Collections;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.Game
{
    public class GameState : LevelFlowState
    {
        [SerializeField]
        private ChickenTankManagement.ChickenTankManager _chickenTankManager = null;
        [SerializeField]
        private TeamManagement.TeamManager _teamManager = null;

        protected override IEnumerator HandleServerSetUpRoutine()
        {
            yield return StartCoroutine(base.HandleServerSetUpRoutine());

            _chickenTankManager.onTankDestroyed += HandleTankDestroyed;
            _chickenTankManager.InitializeForGameplay();
        }

        private void HandleTankDestroyed(ChickenTank tank)
        {
            onStateEnded?.Invoke(this);
            _teamManager.SetLosingTeamData(tank.lifeController.teamController.teamData);
        }

        protected override void HandleServerLeave()
        {
            base.HandleServerLeave();
            _chickenTankManager.CleanUpFromGameplay();
        }
    }
}