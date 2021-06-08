using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    public AudioSource MainMusic;

    public List<AudioSource> MusicAudioSource = new List<AudioSource>();

    public static MusicManager Instance = null;


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
        MainMusic.volume = PlayerPrefs.GetFloat("Music");
        bool RetrySetVolume = false;
        foreach (AudioSource audiosSource in MusicAudioSource)
            {
            if (audiosSource == null)
                {
                MusicAudioSource.Remove(audiosSource);
                RetrySetVolume = true;
                break;
                }
            else
                {
                audiosSource.volume = PlayerPrefs.GetFloat("Music");
                }
            }
        if (RetrySetVolume)
            {
            SetVolume();
            }
        }
    }
