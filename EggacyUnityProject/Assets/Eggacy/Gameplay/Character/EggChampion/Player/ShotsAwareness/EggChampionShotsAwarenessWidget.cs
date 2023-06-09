using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player.ShotsAwareness
{
    public class EggChampionShotsAwarenessWidget : MonoBehaviour
    {
        [SerializeField]
        private float _lifeDuration = 0.5f;
        private float _birthTime = float.MinValue;

        private Vector3 _sourcePosition = default;

        public void SetSourcePosition(Vector3 sourcePosition)
        {
            _sourcePosition = sourcePosition;
        }

        private EggChampionPlayerController _playerController = null;

        public void SetPlayerController(EggChampionPlayerController playerController)
        {
            _playerController = playerController;
        }


        private void Start()
        {
            _birthTime = Time.time;
        }

        private void Update()
        {
            if(Time.time - _birthTime > _lifeDuration)
            {
                Destroy(gameObject);
            }


            var angle = Vector3.SignedAngle(transform.forward, (_sourcePosition - _playerController.character.transform.position).normalized, Vector3.up);

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
};