using Eggacy.Network;
using Fusion;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player
{
    public class EggChampionPlayerController : NetworkBehaviour
    {
        private EggChampionCharacter _character = null;

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (!GetInput(out NetworkedInput input)) return;

            Vector3 directionToMoveCharacter = _character.transform.forward * input.movement.y + _character.transform.right * input.movement.x;
            _character.SetDirectionToMove(directionToMoveCharacter);
        }
    }
}