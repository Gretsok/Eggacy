using Eggacy.Gameplay.Combat.LifeManagement;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player.ShotsAwareness
{
    public class EggChampionShotsAwarenessCanvas : MonoBehaviour
    {
        [SerializeField]
        private EggChampionPlayerController _controller = null;

        [SerializeField]
        private EggChampionShotsAwarenessWidget _widgetPrefab = null;
        [SerializeField]
        private Transform _container = null;

        private void Start()
        {
            _controller.character.lifeController.onDamageTakenFromPosition += HandleDamageTakenFromPosition;
        }

        private void HandleDamageTakenFromPosition(LifeController lifeController, Vector3 sourcePosition)
        {
            var widget = Instantiate(_widgetPrefab, _container);
            widget.SetSourcePosition(sourcePosition);
        }
    }
}