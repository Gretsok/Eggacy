using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Eggacy.Gameplay.Level.WeaponSpawner
{
    public class NotAccessibleWidget : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _timeLeftText = null;
        [SerializeField]
        private Image _timerImage = null;

        public void SetTimeLeft(float timeLeftInSeconds, float totalTimeToWait)
        {
            _timeLeftText.text = $"{timeLeftInSeconds.ToString("0")}";
            _timerImage.fillAmount = timeLeftInSeconds / totalTimeToWait;
        }
    }
}