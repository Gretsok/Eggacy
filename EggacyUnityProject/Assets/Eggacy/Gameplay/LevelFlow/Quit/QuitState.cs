using System.Collections;
using UnityEngine.SceneManagement;

namespace Eggacy.Gameplay.LevelFlow.Quit
{
    public class QuitState : LevelFlowState
    {
        protected override IEnumerator HandleServerSetUpRoutine()
        {
            yield return StartCoroutine(base.HandleServerSetUpRoutine());

            Destroy(Runner.gameObject);
            SceneManager.LoadSceneAsync(0);
        }
    }
}