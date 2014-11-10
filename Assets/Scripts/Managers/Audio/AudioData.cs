using System;
using System.Collections;

namespace Booty.Managers.Audio
{
    [Serializable]
    public class AudioData
    {
        [UnityEngine.SerializeField]
        public string audioName;
        [UnityEngine.SerializeField]
        public UnityEngine.AudioClip audioSource;
        [UnityEngine.SerializeField]
        public float audioVolume;
    }
}