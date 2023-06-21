using Eggacy.Gameplay.Character.EggChampion.Player;
using Eggacy.Gameplay.Combat.LifeManagement;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion
{
    public class LocalEggChampionHealthDisplayManager : MonoBehaviour
    {
        [SerializeField]
        private EggChampionPlayerController _playerController = null;
        [SerializeField]
        private EggChampionHealthBar _healthBar = null;

        private void Start()
        {
            _healthBar.SetHealthRatio(1f);

            _playerController.character.lifeController.onDamageTaken += OnDamageTaken;
            _playerController.character.lifeController.onHealed += OnHealed;
        }

        private void OnDestroy()
        {
            _playerController.character.lifeController.onDamageTaken -= OnDamageTaken;
            _playerController.character.lifeController.onHealed -= OnHealed;
        }

        private void OnHealed(LifeController a_lifeController, int healAMount)
        {
            _healthBar.SetHealthRatio((float)a_lifeController.currentLife / (float)a_lifeController.maxLife);
        }

        private void OnDamageTaken(LifeController a_lifeController, int arg2)
        {
            _healthBar.SetHealthRatio((float)a_lifeController.currentLife / (float)a_lifeController.maxLife);
        }
    }
}