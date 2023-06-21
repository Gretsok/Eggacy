using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Sound
{
    public class MainMusicPlayer : MonoBehaviour
    {
        [SerializeField]
        private SoundEventData m_mainMusic;

        private void Start()
        {
            m_mainMusic.Request2DPlay();
        }
    }
}

