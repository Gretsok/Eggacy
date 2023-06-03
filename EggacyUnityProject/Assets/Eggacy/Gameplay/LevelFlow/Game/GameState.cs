using System.Collections;


namespace Eggacy.Gameplay.LevelFlow.Game
{
    public class GameState : LevelFlowState
    {
        protected override IEnumerator HandleServerSetUpRoutine()
        {
            yield return StartCoroutine(base.HandleServerSetUpRoutine());


        }
    }
}