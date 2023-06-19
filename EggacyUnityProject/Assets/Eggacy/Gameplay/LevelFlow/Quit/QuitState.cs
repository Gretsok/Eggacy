using Fusion;
using System.Collections;

namespace Eggacy.Gameplay.LevelFlow.Quit
{
    public class QuitState : LevelFlowState
    {
        protected override IEnumerator HandleServerSetUpRoutine()
        {
            yield return StartCoroutine(base.HandleServerSetUpRoutine());

            Rpc_LoadMainmenu();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_LoadMainmenu()
        {
            Destroy(Runner.gameObject);
        }
    }
}