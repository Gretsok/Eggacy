using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player
{
    public class EggChampionPlayerCameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _cameraHorizontalPivot = null;
        [SerializeField]
        private Transform _cameraVerticalPivot = null;
        [SerializeField]
        private Transform _referencePointForMovement = null;
        private Vector2 _rotationInput = default;

        public void SetRotationInput(Vector2 rotationInput)
        {
            _rotationInput = rotationInput;
        }

        private void Update()
        {
            _cameraHorizontalPivot.Rotate(_cameraHorizontalPivot.up * _rotationInput.x * Time.deltaTime);
            _cameraVerticalPivot.Rotate(_cameraVerticalPivot.right * _rotationInput.y * Time.deltaTime);
        }
    }
}