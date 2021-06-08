using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public AudioSource MainSFX;
    public List<AudioSource> SFXAudioSource = new List<AudioSource>();

    public static SFXManager Instance = null;


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
        MainSFX.volume = PlayerPrefs.GetFloat("SFX");      

        foreach (AudioSource audiosSource in SFXAudioSource)
            {
            if (audiosSource == null)
                {
                SFXAudioSource.Remove(audiosSource);
                RetrySetVolume = true;
                break;
                }
            else
                {
                audiosSource.volume = PlayerPrefs.GetFloat("SFX");
                }
            }

        if (RetrySetVolume)
            {
            SetVolume();
            }
        }
    }
