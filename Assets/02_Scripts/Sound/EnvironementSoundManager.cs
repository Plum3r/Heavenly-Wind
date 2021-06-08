using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironementSoundManager : MonoBehaviour
{
    public AudioSource MainEnvironement;
    public List<AudioSource> EnvironementAudioSource = new List<AudioSource>();

    public static EnvironementSoundManager Instance = null;


    private void Awake()
        {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (Instance == null)
            {
            Instance = this;
            transform.root.gameObject.AddComponent<DontDestroyOnLoad>();
            }
        else
            {
            Destroy(this.gameObject);
            }
        }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
        Invoke(nameof(SetVolume), 0.5f);
        }

    public void SetVolume()
        {
        bool RetrySetVolume = false;
        MainEnvironement.volume = PlayerPrefs.GetFloat("Environement");

        foreach (AudioSource audiosSource in EnvironementAudioSource)
            {
            if (audiosSource == null)
                {
                EnvironementAudioSource.Remove(audiosSource);
                RetrySetVolume = true;
                break;
                }
            else
                {
                audiosSource.volume = PlayerPrefs.GetFloat("Environement");
                }
            }

        if (RetrySetVolume)
            {
            SetVolume();
            }
        }
    }
