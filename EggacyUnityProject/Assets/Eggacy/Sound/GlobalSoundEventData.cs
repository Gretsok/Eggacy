using System;
using UnityEngine;

namespace Eggacy.Sound
{
    [CreateAssetMenu(fileName = "GlobalSoundEventData", menuName = "Eggacy/Sound/GlobalSoundEventData")]
    public class GlobalSoundEventData : ASoundEventData
    {

        public Action<GlobalSoundEventData> onPlayRequested;
        public void RequestPlay()
        {
            onPlayRequested?.Invoke(this);
        }

    }
}