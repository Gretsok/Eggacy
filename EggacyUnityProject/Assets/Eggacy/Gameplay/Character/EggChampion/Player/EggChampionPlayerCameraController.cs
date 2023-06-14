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
        public Transform referencePointForMovement => _referencePointForMovement;
        [SerializeField]
        private Camera _camera = null;
        public new Camera camera => _camera;


        [SerializeField]
        private Vector2 _cameraVerticalClamping = new Vector2(-50, 85);


        private float _deltaVerticalOrientation = default;

        public void SetRotationInput(float deltaVerticalOrientation)
        {
            _deltaVerticalOrientation = deltaVerticalOrientation;

            var eulerAngles = _cameraVerticalPivot.localEulerAngles;
            eulerAngles.x += -_deltaVerticalOrientation;

            if (eulerAngles.x > _cameraVerticalClamping.y && eulerAngles.x < 180f)
            {
                eulerAngles.x = _cameraVerticalClamping.y;
            }
            else if (eulerAngles.x < 360f + _cameraVerticalClamping.x && eulerAngles.x > 180f)
            {
                eulerAngles.x = 360f + _cameraVerticalClamping.x;
            }
            _cameraVerticalPivot.localEulerAngles = eulerAngles;
        }
    }
}