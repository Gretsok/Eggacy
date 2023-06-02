using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player
{
    public class EggChampionPlayerCameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _rootPivot = null;
        [SerializeField]
        private Transform _cameraHorizontalPivot = null;
        [SerializeField]
        private Transform _cameraVerticalPivot = null;
        [SerializeField]
        private Transform _referencePointForMovement = null;
        public Transform referencePointForMovement => _referencePointForMovement;


        [SerializeField]
        private Vector2 _cameraSensitivity = new Vector2(30f, 30f);
        [SerializeField]
        private Vector2 _cameraVerticalClamping = new Vector2(-50, 85);

        private Transform _positionTarget = null;

        private Vector2 _rotationInput = default;

        public void SetPositionTarget(Transform positionTarget)
        {
            _positionTarget = positionTarget;
        }

        public void SetRotationInput(Vector2 rotationInput)
        {
            _rotationInput = rotationInput;
        }

        private void FixedUpdate()
        {
            if (!gameObject.activeSelf) return;
            if(!_positionTarget || !_rootPivot) return;

            _cameraHorizontalPivot.Rotate(Vector3.up * _rotationInput.x * Time.fixedDeltaTime * _cameraSensitivity.x);

            var eulerAngles = _cameraVerticalPivot.localEulerAngles;
            eulerAngles.x += -_rotationInput.y * Time.fixedDeltaTime * _cameraSensitivity.y;

            if (eulerAngles.x > _cameraVerticalClamping.y && eulerAngles.x < 180f)
            {
                eulerAngles.x = _cameraVerticalClamping.y;
            }
            else if (eulerAngles.x < 360f + _cameraVerticalClamping.x && eulerAngles.x > 180f)
            {
                eulerAngles.x = 360f + _cameraVerticalClamping.x;
            }

            _cameraVerticalPivot.localEulerAngles = eulerAngles;

            _rootPivot.position = _positionTarget.position;
        }
    }
}