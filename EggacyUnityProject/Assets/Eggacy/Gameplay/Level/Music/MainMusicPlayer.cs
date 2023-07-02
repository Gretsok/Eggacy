using Eggacy.Sound;
using UnityEngine;

namespace Eggacy.Gameplay.Level.Music
{
    public class MainMusicPlayer : MonoBehaviour
    {
        [SerializeField]
        private GlobalSoundEventData m_mainMusic;

        private void Start()
        {
            m_mainMusic.RequestPlay();
        }
    }
}

