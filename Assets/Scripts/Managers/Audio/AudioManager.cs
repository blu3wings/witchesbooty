using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Booty.Managers.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager instance
        {
            get { return _instance; }
        }

        public List<AudioData> sfxSources;
        public List<AudioData> bgmSources;
        public List<AudioData> ambientSources;

        private void Awake()
        {
            _instance = this;
            GameObject.DontDestroyOnLoad(this);
        }

        public void playAudio()
        {

        }

        public void addAmbientAudioData(string audioName)
        {
            if (ambientSources == null)
                ambientSources = new List<AudioData>();

            AudioData data = new AudioData();
            data.audioName = audioName;
            ambientSources.Add(data);
        }

        public void deleteAmbientAudioData(int index, Action callback)
        {
            if (ambientSources == null) return;
            if (ambientSources.Count < 1) return;

            ambientSources.RemoveAt(index);

            if (callback != null)
                callback();
        }

        public void addBgmAudioData(string audioName)
        {
            if (bgmSources == null)
                bgmSources = new List<AudioData>();

            AudioData data = new AudioData();
            data.audioName = audioName;
            bgmSources.Add(data);
        }

        public void deleteBgmAudioData(int index, Action callback)
        {
            if (bgmSources == null) return;
            if (bgmSources.Count < 1) return;

            bgmSources.RemoveAt(index);

            if (callback != null)
                callback();
        }

        public void addSfxAudioData(string audioName)
        {
            if (sfxSources == null)
                sfxSources = new List<AudioData>();

            AudioData data = new AudioData();
            data.audioName = audioName;
            sfxSources.Add(data);
        }

        public void deleteSfxAudioData(int index, Action callback)
        {
            if (sfxSources == null) return;
            if (sfxSources.Count < 1) return;

            sfxSources.RemoveAt(index);

            if (callback != null)
                callback();
        }
    }
}