using Eggacy.Gameplay.Level.WeaponSpawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Level
{
    public class HealSpawnerFeedbackHandler : MonoBehaviour
    {
        [SerializeField]
        private HealSpawner _spawner = null;

        [Space]
        [SerializeField]
        private MeshRenderer _renderer = null;
        [SerializeField]
        private Material _activatedMaterial, _deactivatedMaterial = null;

        [Space]
        [SerializeField]
        private ParticleSystem _activatedFX = null;

        [SerializeField]
        private NotAccessibleWidget _notAccessibleWidget = null;

        [SerializeField]
        private Transform _modelContainer = null;
        private GameObject _model = null;

        private bool _isActive = false;

        private void Start()
        {
            SetUp();
            DisplayDeactivation();
        }

        private void SetUp()
        {
            _model = Instantiate(_spawner.weaponModelControllerPrefab, _modelContainer);
        }

        private void Update()
        {
            if (!_spawner.Runner) return;

            if (_isActive)
            {
                if (!_spawner.isActive)
                {
                    DisplayDeactivation();
                }
            }
            else
            {
                _notAccessibleWidget.SetTimeLeft(_spawner.cooldownToRespawn - _spawner.timePassedSincePickUp, _spawner.cooldownToRespawn);
                if (_spawner.isActive)
                {
                    DisplayActivation();
                }
            }
        }

        private void DisplayActivation()
        {
            _isActive = true;

            _activatedFX.Play();
            _notAccessibleWidget.gameObject.SetActive(false);
            _renderer.material = _activatedMaterial;
            _model.gameObject.SetActive(true);
        }

        private void DisplayDeactivation()
        {
            _isActive = false;

            _activatedFX.Stop();
            _notAccessibleWidget.gameObject.SetActive(true);
            _renderer.material = _deactivatedMaterial;
            _model.gameObject.SetActive(false);
        }
    }
}
