using DG.Tweening;
using UnityEngine;

namespace Eggacy.Gameplay.Level.WeaponSpawner
{
    public class WeaponModelContainerTweener : MonoBehaviour
    {
        [SerializeField]
        private float _heightAmplitude = 1f, _heightPeriod = 1f, _rotationSpeed = 1f;

        void Start()
        {
            StartMoveYTweenUp();
        }

        private void Update()
        {
            transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
        }

        public void StartMoveYTweenUp()
        {
            transform.DOLocalMoveY(_heightAmplitude, _heightPeriod / 2f).SetEase(Ease.InOutQuad).OnComplete(StartMoveYTweenDown);
        }

        public void StartMoveYTweenDown()
        {
            transform.DOLocalMoveY(0, _heightPeriod / 2f).SetEase(Ease.InOutQuad).OnComplete(StartMoveYTweenUp);
        }
    }
}