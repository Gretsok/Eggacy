using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Eggacy.Gameplay.LevelFlow.ChickenTankManagement.UI
{
    public class TankHealthTeamDisplayContainer : MonoBehaviour
    {
        [SerializeField]
        private Image _healthBar = null;
        [SerializeField]
        private TextMeshProUGUI _teamName = null;

        public void SetColor(Color color)
        {
            Color barColor = color;
            barColor.a = 1f;
            _healthBar.color = barColor;

            _teamName.color = color;
        }

        public void SetName(string name)
        {
            _teamName.text = name;
        }

        public void SetHealthRatio(float healthRatio)
        {
            _healthBar.fillAmount = healthRatio;
        }
    }
}