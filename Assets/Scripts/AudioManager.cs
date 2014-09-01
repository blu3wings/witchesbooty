using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    private AudioSource _audio;

    private static AudioManager _instance;
    public static AudioManager instance
    {
        get { return _instance; }
    }

    public AudioClip[] clips;

    private void Awake()
    {
        _instance = this;
        _audio = gameObject.GetComponent<AudioSource>();
    }

    public void play(int id,bool loop)
    {
        AudioClip clip = clips[id];
        if(loop)
        {
            _audio.clip = clip;
            _audio.Play();
        }
        else
        {
            _audio.PlayOneShot(clip);
        }        
    }

    public void stop()
    {
        _audio.Stop();
    }
}