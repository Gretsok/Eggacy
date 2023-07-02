using UnityEngine;

namespace Eggacy.Sound
{
    public class EggChampionDieSoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private GlobalSoundEventData m_diedSound;

        private void Start()
        {
            
        }

        public void PlayDiedSound()
        {
            m_diedSound.RequestPlay();
        }
    }
}

