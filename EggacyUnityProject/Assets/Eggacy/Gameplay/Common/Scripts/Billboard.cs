using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay
{
    public class Billboard : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if(_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            if(_mainCamera == null)
            {
                return;
            }

            Vector3 lookDirection = transform.position - _mainCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = targetRotation;
        }
    }
}

