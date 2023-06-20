using Eggacy.Gameplay.Combat.LifeManagement;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Player.DeathDisplay
{
    public class EggChampionPlayerDeathDisplayHandler : MonoBehaviour
    {
        [SerializeField]
        private EggChampionPlayerController _controller;
        [SerializeField]
        private Transform _container = null;
        [SerializeField]
        private TextMeshProUGUI _deathCooldownText = null;

        private void Start()
        {
            _container.gameObject.SetActive(false);
            _controller.Character.lifeController.onDied += HandleDied;
        }

        private void HandleDied(LifeController obj)
        {
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            float startTime = Time.time;
            _container.gameObject.SetActive(true);
            while(Time.time - startTime < _controller.Character.respawnDuration)
            {
                _deathCooldownText.text = (_controller.Character.respawnDuration - (Time.time - startTime)).ToString("0.0");
                yield return null;
            }
            _container.gameObject.SetActive(false);
        }
    }
}
