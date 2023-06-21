using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player.ShotsAwareness
{
    public class EggChampionShotsAwarenessWidget : MonoBehaviour
    {
        private Vector3 _sourcePosition = default;

        public void SetSourcePosition(Vector3 sourcePosition)
        {
            _sourcePosition = sourcePosition;
        }
    }
}