using Fusion;

namespace Eggacy.Gameplay.Character.ChickenTank.Behaviour
{
    public class AChikenTankBehaviourState : NetworkBehaviour
    {
        public void EnterState()
        {
            if (!Runner.IsServer) return;

            HandleEnter();
        }
        protected virtual void HandleEnter()
        { }

        public void UpdateState()
        {
            if(!Runner.IsServer) return;

            HandleUpdate();
        }

        protected virtual void HandleUpdate()
        { }

        public void LeaveState()
        {
            if (!Runner.IsServer) return;

            HandleLeave();
        }

        protected virtual void HandleLeave()
        {

        }
    }
}