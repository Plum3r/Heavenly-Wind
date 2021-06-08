using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterToManager : MonoBehaviour
{
    public AudioType AudioSourceType;

    void Start()
    {
        if(AudioSourceType == AudioType.Music)
            {
            MusicManager.Instance.MusicAudioSource.Add(this.GetComponent<AudioSource>());
            MusicManager.Instance.SetVolume();
            }
        if (AudioSourceType == AudioType.SFX)
            {
            SFXManager.Instance.SFXAudioSource.Add(this.GetComponent<AudioSource>());
            SFXManager.Instance.SetVolume();
            }
        if (AudioSourceType == AudioType.Environement)
            {
            EnvironementSoundManager.Instance.EnvironementAudioSource.Add(this.GetComponent<AudioSource>());
            EnvironementSoundManager.Instance.SetVolume();
            }

        }
    public enum AudioType
        {
        SFX,
        Music,
        Environement
        }
}
