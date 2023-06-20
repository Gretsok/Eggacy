using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Sound
{
    public class EggChampionDieSoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private SoundEventData m_diedSound;

        private void Start()
        {
            
        }

        public void PlayDiedSound()
        {
            m_diedSound.Request2DPlay();
        }
    }
}

